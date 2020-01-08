using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    Enemy enemyInfo = null;
    [SerializeField]
    SpriteRenderer spriteRef = null, headRef = null, bodyRef = null, legsRef = null;
    [SerializeField]
    Text enemyHealthText = null, attackSequenceText = null, enemyNameText = null;
    [SerializeField]
    Slider enemyHealthBar = null, playerHealthBar = null;
    [SerializeField]
    List<Reactions> currentEnemyReactions = new List<Reactions>();
    [SerializeField]
    List<Sprite> headList = new List<Sprite>(), bodyList = new List<Sprite>(), legsList = new List<Sprite>();

    int prevAttack = -1;
    public List<int> attackSequence = new List<int>();
    [Header("Test")]
    [SerializeField]
    KeyCode testKey = KeyCode.T;
    [SerializeField]
    bool test = false, debugging = false;
    [Header("Menu")]
    [SerializeField]
    GameObject pauseMenu = null;
    [SerializeField]
    KeyCode pauseKey = KeyCode.Escape;

    float testDelay = 0, winDelay = 0, colorDelay = 0;
    Color startColor;
    bool win = false;
    float playerHp = 5, enemyHp = 0;

    // Start is called before the first frame update
    void Start()
    {
        NewEnemy();
        if (spriteRef != null) startColor = spriteRef.color;
        if (playerHealthBar != null) playerHealthBar.maxValue = playerHp;
    }

    // Update is called once per frame
    void Update()
    {
        if ((test && Input.GetKey(testKey)) && testDelay < Time.time)
        {
            NewEnemy();
            testDelay = Time.time + 0.5f;
        }
        if (winDelay > 0 && winDelay < Time.time)
        {
            win = false;
            ClearSequence();
            NewEnemy();
            winDelay = 0;
        }
        if (colorDelay > 0 && colorDelay < Time.time)
        {
            spriteRef.color = startColor;
            colorDelay = 0;
        }
        if (Input.GetKeyDown(pauseKey))
        {
            ToggleMenu(pauseMenu);
        }
    }
    public void NewEnemy()
    {
        enemyInfo = new Enemy(Random.Range(0, 5));
        if (enemyNameText != null) enemyNameText.text = enemyInfo.enemyName;
        if (enemyHealthText != null) enemyHealthText.text = "HP: " + enemyInfo.reactions.Count;
        enemyHp = enemyInfo.reactions.Count;
        if (enemyHealthBar != null) enemyHealthBar.maxValue = enemyHp;
        if (debugging)
        {
            for (int i = 0; i < enemyInfo.reactions.Count; i++)
            {
                for (int c = 0; c < enemyInfo.reactions[i].reactionSet.Count; c++)
                {
                    Debug.Log(enemyInfo.enemyName + " " + i + "-" + c + ":" + enemyInfo.reactions[i].reactionSet[c]);
                }
            }
        }
        currentEnemyReactions = enemyInfo.reactions;
        RestorePlayerHealth();
        StartCoroutine("UpdateHealth");
        RandomizeEnemyParts();
    }
    public void RandomizeEnemyParts()
    {
        if (headList.Count > 1)
        {
            if (headRef != null) headRef.sprite = headList[Random.Range(0, headList.Count)];
        }
        if (bodyList.Count > 1)
        {
            if (bodyRef != null) bodyRef.sprite = bodyList[Random.Range(0, bodyList.Count)];
        }
        if (legsList.Count > 1)
        {
            if (legsRef != null) legsRef.sprite = legsList[Random.Range(0, legsList.Count)];
        }
    }
    public void AddCombo(int type)
    {
        if (type == prevAttack) return;
        if (!win)
        {
            attackSequence.Add(type);
        }
        UpdateAttackSequence();
        prevAttack = type;
    }
    public void ClearSequence()
    {
        attackSequence.Clear();
        UpdateAttackSequence();
        prevAttack = -1;
    }
    public int CheckSequence()
    {
        int interrupted = -1;
        for (int i = 0; i < enemyInfo.reactions.Count; i++)
        {
            if (i < attackSequence.Count)
            {
                if (enemyInfo.reactions[i].reactionSet.Contains(attackSequence[i]))
                    interrupted = i;
            }
        }
        return interrupted;
    }
    public void TakeDamage()
    {
        int failedAttack = CheckSequence() - 1;
        Debug.Log(failedAttack);
        if (failedAttack > -1)
        {
            for (int i = attackSequence.Count - 1; i > 0; i--)
            {
                if (i > failedAttack)
                    attackSequence.RemoveAt(i);
            }
            if (spriteRef != null) spriteRef.color = Color.red;
            colorDelay = Time.time + 1;
            playerHp--;
            Debug.Log("Player HP: " + playerHp);
        }
        else
        {
            if (CheckWin())
            {
                if (!win)
                {
                    win = !win;
                    if (spriteRef != null) spriteRef.color = Color.green;
                    colorDelay = Time.time + 1;
                    winDelay = Time.time + 3;
                }
            }
        }
        UpdateAttackSequence();
        enemyHp = enemyInfo.reactions.Count - attackSequence.Count;
        Debug.Log("Enemy HP: " + enemyHp);
        StartCoroutine("UpdateHealth");
    }
    public bool CheckWin()
    {
        bool win = false;
        win = attackSequence.Count >= enemyInfo.reactions.Count;
        return win;
    }
    void UpdateAttackSequence()
    {
        string tempText = "Attacks: ";
        for (int i = 0; i < attackSequence.Count; i++)
        {
            if (i > 0)
            {
                tempText += "/";
            }
            if (debugging)
            {
                tempText += attackSequence[i];
            }
            else
            {
                string attackName = "";
                switch (attackSequence[i])
                {
                    case 0:
                        attackName = "P";
                        break;
                    case 1:
                        attackName = "K";
                        break;
                    case 2:
                        attackName = "T";
                        break;
                    case 3:
                        attackName = "G";
                        break;
                }
                tempText += attackName;
            }
        }
        if (attackSequenceText != null) attackSequenceText.text = tempText;
    }
    void RestorePlayerHealth()
    {
        playerHp = 5;
    }
    IEnumerator UpdateHealth()
    {
        while (Mathf.Abs(enemyHealthBar.value - enemyHp) > 0.01f)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemyHealthBar != null)
            {
                enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, enemyHp, 10);
            }
        }
        while (Mathf.Abs(playerHealthBar.value - playerHp) > 0.01f)
        {
            yield return new WaitForSeconds(0.5f);
            if (playerHealthBar != null)
            {
                playerHealthBar.value = Mathf.Lerp(playerHealthBar.value, playerHp, 3);
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleMenu(GameObject menu)
    {
        if (menu != null) menu.SetActive(!menu.activeInHierarchy);
    }
}
