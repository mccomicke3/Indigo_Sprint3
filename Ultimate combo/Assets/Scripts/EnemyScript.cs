using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*-------------------------------------------------------------------------
 * EnemyScript
 * script to control the behaviour of the current active enemy, and will
 * use the gui manager to update the various gui elements.
 * To be attached to: The first enemy in the scene
 * Responsible for:
 * 
 * reseting the enemy data when you need new enemy entities
 * keeping track of the known combos
 * handling the main gameplay loop
 * keeping track of player HP
 * keeping track of entered moves
 * dealing damage to the enemy
 * 
 * Will call the GUI manager to update the GUI when nescessary
 * 
-------------------------------------------------------------------------*/

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    GUIManager guiManager = null; //This is the GUI script

    [SerializeField]
    Enemy enemyInfo = null; //Holds the information about the current enemy

    [SerializeField]
    List<string> currentWeaknesses = new List<string>(); //The current list of weaknesses

    [SerializeField]
    List<Sprite> headList = new List<Sprite>(), bodyList = new List<Sprite>(), legsList = new List<Sprite>();

    int prevAttack = -1; //indicates the most recent move input by the user -1 is none
    public List<int> attackSequence = new List<int>();
    public List<string> knownWeaknesses = new List<string>();
    //holds the number of moves that a user enters each turn

    [Header("Test")]
    [SerializeField] //testing tool used for testing some features
    KeyCode testKey = KeyCode.T;

    [SerializeField]
    bool test = false, debugging = false, timedTurn = true;

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
            enemyInfo.weaknesses = enemyInfo.WeaknessGenerator();
            currentWeaknesses = enemyInfo.weaknesses;
        }
        if (playerHp <= 0)
        {
            PlayerEnd(false);
        }
    }

    //Generates a New enemy object

    public void NewEnemy()
    {
        enemyInfo = new Enemy();
        guiManager.SetEnemyInfo(enemyInfo);
        enemyHp = 100;
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
    /*
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
    */
    public void TakeDamage()
    { 
    /*
     * will have to determine if the player will take damage?
     * 
     * 
    {
        if (timedTurn)
        {
            StopCoroutine("TimedTurn");
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
    
    */}

    public bool CheckWin()
    {
        if (enemyInfo.enemyHp <= 0) {
            return true;
        }
        return false;
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


    /* Declares a coroutine to keep track of the limited time between turns 
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
