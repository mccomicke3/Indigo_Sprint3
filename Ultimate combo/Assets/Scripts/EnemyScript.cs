using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    GUIManager guiManager = null;
    [SerializeField]
    Enemy enemyInfo = null;
    [SerializeField]
    List<Reactions> currentEnemyReactions = new List<Reactions>();
    [Tooltip("x is combo length, y is number of weaknesses")]
    [SerializeField]
    Vector2Int weaknessParam = new Vector2Int();
    [SerializeField]
    List<Weaknesses> currentWeaknesses = new List<Weaknesses>();
    //list of weaknesses
    [SerializeField]
    List<Sprite> headList = new List<Sprite>(), bodyList = new List<Sprite>(), legsList = new List<Sprite>();

    int prevAttack = -1;
    public List<int> attackSequence = new List<int>();
    //holds the number of moves that a user enters each turn

    [Header("Test")]
    [SerializeField]
    KeyCode testKey = KeyCode.T;
    [SerializeField]
    bool test = false, debugging = false, timedTurn = false, useAmmo = false, reuseAttacks = false;
    [SerializeField]
    int ammo = 0, startAmmo = 50;
    [SerializeField, Tooltip("Time in seconds")]
    float timedTurnLength = 60;
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
        if (guiManager != null) guiManager.SetPlayerMaxHP(playerHp);

        if (timedTurn)
        {
            StartCoroutine("TimedTurn");
        }
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("weak!");
            enemyInfo.weaknesses = enemyInfo.NewWeaknesses(weaknessParam.x,weaknessParam.y);
            currentWeaknesses = enemyInfo.weaknesses;
        }
        if (playerHp <= 0)
        {
            PlayerEnd(false);
        }
    }
    public void NewEnemy()
    {
        ammo = startAmmo;
        if (useAmmo) guiManager.SetAmmoText(ammo);
        enemyInfo = new Enemy(Random.Range(0, 5));
        guiManager.SetEnemyInfo(enemyInfo);
        enemyHp = enemyInfo.reactions.Count;
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
        currentWeaknesses = enemyInfo.weaknesses;
        RestorePlayerHealth();
        guiManager.StartUpdateHealth(enemyHp, playerHp);
        RandomizeEnemyParts();
    }

    public void RandomizeEnemyParts()
    {
        if (headList.Count > 1)
        {
             guiManager.EnemyBodySprite(GUIManager.BodyPart.Head, headList[Random.Range(0, headList.Count)]);
        }
        if (bodyList.Count > 1)
        {
            guiManager.EnemyBodySprite(GUIManager.BodyPart.Body, bodyList[Random.Range(0, bodyList.Count)]);
        }
        if (legsList.Count > 1)
        {
            guiManager.EnemyBodySprite(GUIManager.BodyPart.Legs, legsList[Random.Range(0, legsList.Count)]);
        }
    }


    /*
     * Takes integer representing the move that a user inputs and adds it to
     * the list attackSequence
     * returns void
     */

    public void AddCombo(int type)
    {
        if (!reuseAttacks && type == prevAttack) return;
        if (!win)
        {
            attackSequence.Add(type);
        }
        UpdateAttackSequence();
        prevAttack = type;
    }
    /* 
     *
     */
    public void ClearSequence()
    {
        attackSequence.Clear();
        UpdateAttackSequence();
        prevAttack = -1;
    }

    public int CheckSequence()
    {
        int interrupted = -1;
        // go through each reaction on the enemy
        for (int i = 0; i < enemyInfo.reactions.Count; i++)
        {
            // check if it is within the range of the attack sequence
            if (i < attackSequence.Count)
            {
                // check if the enemy reacts to the specific attack sequence
                if (enemyInfo.reactions[i].reactionSet.Contains(attackSequence[i]))
                    interrupted = i;
            }
        }
        return interrupted;
    }
    public void TakeDamage()
    {
        if (timedTurn)
        {
            StopCoroutine("TimedTurn");
        }
        if (useAmmo)
        {
            ammo -= attackSequence.Count;
            guiManager.SetAmmoText(ammo);
            if (ammo <= 0)
            {
                ammo = 0;
                PlayerEnd(false);
            }
        }
        int failedAttack = CheckSequence() - 1;
        Debug.Log(failedAttack);
        // if the attack got interrupted
        if (failedAttack > -1)
        {
            for (int i = attackSequence.Count - 1; i > 0; i--)
            {
                if (i > failedAttack)
                    attackSequence.RemoveAt(i);
            }
            guiManager.SetSpriteColor(Color.red);
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
                    guiManager.SetSpriteColor(Color.green);
                    winDelay = Time.time + 3;
                }
            }
        }
        ClearSequence();
        UpdateAttackSequence();
        enemyHp = enemyInfo.reactions.Count - attackSequence.Count;
        Debug.Log("Enemy HP: " + enemyHp);
        guiManager.StartUpdateHealth(enemyHp, playerHp);

        if (timedTurn)
        {
            StartCoroutine("TimedTurn");
        }
    }
    public bool CheckWin()
    {
        bool win = false;
        win = attackSequence.Count >= enemyInfo.reactions.Count;
        return win;
    }

    void PlayerEnd(bool win)
    {
        if (timedTurn) StopCoroutine("TimedTurn");
        guiManager.EndGame(win);
    }

    /* Updates the text which appears at the top of the screen representing
     * The sequence of moves which a player enters each turn.
     * returns void
     */
    void UpdateAttackSequence()
    {
        string tempText = "Attacks: "; //holds the current moves entered
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
        guiManager.UpdateAttackSequenceText(tempText);
    }

    void RestorePlayerHealth()
    {
        playerHp = 5;
        guiManager.StartUpdateHealth(enemyHp, playerHp);
    }


    /* Declares a coroutine for 
     */

    IEnumerator TimedTurn()
    {
        float tempTime = timedTurnLength;
        while (timedTurn)
        {
            guiManager.SetTimedTurn(tempTime);
            tempTime--;
            if (tempTime < 1)
            {
                playerHp--;
                guiManager.StartUpdateHealth(enemyHp, playerHp);
                tempTime = timedTurnLength;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
