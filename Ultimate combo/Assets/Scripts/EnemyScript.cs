using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    Enemy enemyInfo = null;
    [SerializeField]
    SpriteRenderer spriteRef = null;
    [SerializeField]
    Text enemyHealthText = null, attackSequenceText = null, enemyNameText = null;

    int prevAttack = -1;
    List<int> attackSequence = new List<int>();
    [Header("Test")]
    [SerializeField]
    bool test = false;
    [SerializeField]
    KeyCode testKey = KeyCode.T;

    float testDelay = 0, winDelay = 0;
    bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        NewEnemy();
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
    }
    public void NewEnemy()
    {
        bool debugging = true;
        enemyInfo = new Enemy(Random.Range(0, 5));
        if (enemyNameText != null) enemyNameText.text = enemyInfo.enemyName;
        if (enemyHealthText != null) enemyHealthText.text = "HP: " + enemyInfo.reactions.Count;
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
    }
    public void AddCombo(int type)
    {
        if (type == prevAttack) return;
        attackSequence.Add(type);
        if (CheckSequence())
        {
            TakeDamage();
        }
        else
        {
            if (CheckWin())
            {
                Debug.Log("You Ween");
                if (!win)
                {
                    win = !win;
                    winDelay = Time.time + 3;
                }
                else
                    attackSequence.RemoveAt(attackSequence.Count - 1);
            }
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
    public bool CheckSequence()
    {
        bool interrupted = false;
        for (int i = 0; i < enemyInfo.reactions.Count; i++)
        {
            if (i < attackSequence.Count)
            {
                interrupted = (enemyInfo.reactions[i].reactionSet.Contains(attackSequence[i]));
            }
        }
        return interrupted;
    }
    public void TakeDamage()
    {
        Debug.Log("You got attacked");
        attackSequence.RemoveAt(attackSequence.Count - 1);
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
        for (int i = 0; i < attackSequence.Count;i++)
        {
            if (i > 0)
            {
                tempText += "/";
            }
            tempText += attackSequence[i];
        }
        if (attackSequenceText != null) attackSequenceText.text = tempText;
    }
}
