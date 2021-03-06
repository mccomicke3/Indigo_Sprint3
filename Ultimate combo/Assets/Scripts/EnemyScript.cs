﻿using System.Collections;
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
    Audio audiomanager = null; //the audio manager

    [SerializeField]
    Enemy enemyInfo = null; //Holds the information about the current enemy

    [SerializeField]
    List<string> currentWeaknesses = new List<string>(); //The current list of weaknesses

    [SerializeField]
    List<Sprite> headList = new List<Sprite>(), bodyList = new List<Sprite>(), legsList = new List<Sprite>();

    [Header("Test")]
    [SerializeField] //testing tool used for testing some features
    KeyCode testKey = KeyCode.T;

    [SerializeField]
    bool test = false, debugging = false;

    [SerializeField, Tooltip("Time in seconds")]
    float timedTurnDefault = 15;
    float timedTurnLength;

    [SerializeField]
    KeyCode pauseKey = KeyCode.Escape;
    public string attackSequence = ""; //represents the sequence of entered moves
    public List<string> knownWeaknesses = new List<string>();
    //knownweaknesses represents the list of weaknesses known to the user. 
    public enum Gamestate {Intro, Victory, Loss, Combat, Menu};

    public int gamestate = (int)Gamestate.Combat;

    float testDelay = 0.5f, winDelay = 0;
    float playerHp = 50;

    int parrycount = 0;




    // Start is called before the first frame update
    void Start()
    {
        guiManager.SetPlayerMaxHP(playerHp);
        RestorePlayerHealth();
        enemyInfo.enemyHp = 200;
        timedTurnLength = timedTurnDefault;
        if (enemyInfo == null) NewEnemy();
        else
        {
            guiManager.SetEnemyInfo(enemyInfo);
            currentWeaknesses = enemyInfo.weaknesses;
            guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
            knownWeaknesses = enemyInfo.CensoredWeaknesses();
        }

        StartCoroutine("TimedTurn");
        
    }

    // Update is called once per frame
    void Update()
    {

        if ((test && Input.GetKey(testKey)) && testDelay < Time.time)
        {
            Dictionary<string, int> testdict = enemyInfo.TotalNumCombo(attackSequence);
            foreach(string weakness in enemyInfo.weaknesses)
            {
                Debug.Log(weakness + ": " + testdict[weakness].ToString());
            }
            testDelay = Time.time + testDelay;
        }

        if (Input.GetKeyDown(pauseKey))
        {
            guiManager.ToggleMenu();
        }

    }

    //Generates a New enemy object

    public string ComboNumToString(string moves)
    {
        string tempText = ""; //holds the current moves entered
        for (int i = 0; i < moves.Length; i++)
        {
            if (i > 0)
            {
                tempText += " ";
            }


            string attackName = "";
            switch (moves[i])
            {
                case '0':
                    attackName = "P";
                    break;
                case '1':
                    attackName = "K";
                    break;
                case '2':
                    attackName = "Py";
                    break;
                case '3':
                    attackName = "T";
                    break;
                case '*':
                    attackName = "*";
                    break;

            }
            tempText = tempText + attackName;
        }
        return tempText;
    }

    public void NewEnemy()
    {
        enemyInfo = ScriptableObject.CreateInstance<Enemy>();
        enemyInfo.Randomize();
        guiManager.SetEnemyInfo(enemyInfo);
        currentWeaknesses = enemyInfo.weaknesses;
        RestorePlayerHealth();
        guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
        RandomizeEnemyParts();
        knownWeaknesses = enemyInfo.CensoredWeaknesses();
        timedTurnLength = timedTurnDefault;
    }

    //randomizes enemy parts
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


    /*-------------------------------------------------------------------------
    adds a single move as an string holding a value between between 0 - 3
    also updates the move in the display.
    to be called by the "move buttons"
    -------------------------------------------------------------------------*/

    public void AddCombo(string move)
    {
        if (gamestate == 3 && (attackSequence.Length < 8))
        {
            //if (attackSequence.Length == 7 )guiManager.ShowEnemyHighlight(true);
            attackSequence = attackSequence + move;
        }
        //Debug.Log(move);
        UpdateAttackSequence();
    }


    /*-------------------------------------------------------------------------
     * clears and updates the attack sequence 
    -------------------------------------------------------------------------*/
    public void ClearSequence()
    {
        attackSequence = "";
        UpdateAttackSequence();
    }

    public void Enemyturn()
    {
        guiManager.ShowEnemyHighlight(false);

        StopCoroutine("TimedTurn");
        
        timedTurnLength = timedTurnDefault;



        if (debugging)
        {
            Debug.Log("--------------actual weaknesses----------------");
            knownWeaknesses = enemyInfo.UpdateKnownWeaknesses(knownWeaknesses, "");
            foreach (string weakness in enemyInfo.weaknesses) Debug.Log(weakness);

            //Debug.Log("--------------before knownweaknesses----------------: ");
            //foreach (string weakness in knownWeaknesses) Debug.Log(weakness);

            Debug.Log("attackinput: " + attackSequence);
            Debug.Log("Number of combos contained: " + enemyInfo.IsCombo(attackSequence));
            Debug.Log("Is final combo : " + enemyInfo.IsFinalCombo(attackSequence));
        }


        knownWeaknesses = enemyInfo.UpdateKnownWeaknesses(knownWeaknesses, attackSequence);
        guiManager.UpdateKnownComboText(knownWeaknesses);
        if (debugging) { 

            Debug.Log("--------------after knownweaknesses----------------: ");
            foreach (string weakness in knownWeaknesses) Debug.Log(weakness);
        }

        knownWeaknesses = enemyInfo.UpdateKnownWeaknesses(knownWeaknesses, attackSequence);


        StartCoroutine(DealDamage(attackSequence));
        ClearSequence();
        
        if (CheckWin())
        {
            if (gamestate == 1) //victory
            {
                guiManager.SetSpriteColor(Color.green);
                winDelay = Time.time + 3;
            }
        }


        UpdateAttackSequence();
        //enemyHp = enemyHp - 20;
        guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);


        
    }

    /*-------------------------------------------------------------------------
     * Deals an amount of damage for each attack to the enemy,
     * Will give an aplified effect upon completion of the subcombos, 
     * if the inputed string has all subcombos, it will deal a large amount 
     * of damage.
     * repsonsible for:
     * updating damage text
     * dealing damage
     * 
     * returns void
    -------------------------------------------------------------------------*/

    IEnumerator DealDamage(string inputcombo)
    {
        StopCoroutine("TimedTurn");
        string potentialcombo = "";
        int damagedealt;
        int punchbasedamage = 4;
        int kickbasedamage = 3;
        int parrybasedamage = 1;
        int tauntbasedamage = 0;
        int punchcomboscaling = 6;
        int kickcomboscaling = 8;
        int parrycomboscaling = 6;
        int tauntcomboscaling = 1;
        int parrycount = 0;

        bool islastcombomove = false;

        Dictionary<string, int> comboinfo;
        Dictionary<string, int> prevcomboinfo = null;

        int attack = 0; //for testing

        foreach(char move in inputcombo)
        {

            damagedealt = 0;
            potentialcombo = potentialcombo + move;
            comboinfo = enemyInfo.TotalNumCombo(potentialcombo);

            islastcombomove = false;
            //attack += 1; //for testing
            //Debug.Log("attack number: " + attack.ToString());//for testing
            audiomanager.Punch();
            switch (move)
            {

                case '0': //punch
                    foreach (string weakness in enemyInfo.weaknesses)//determining if there is a new combo
                    {
                        if (prevcomboinfo == null) break;
                        if (comboinfo[weakness] > prevcomboinfo[weakness])//combo acheived
                        {
                            damagedealt += (punchcomboscaling * weakness.Length); //combo effect
                            damagedealt += 10; // punch combos just deal extra flat damage.
                            guiManager.UpdateFlavourText("Punch Combo\n" + ComboNumToString(weakness) + "\nExtra Damage!");
                            yield return new WaitForSeconds(1f);
                            guiManager.UpdateFlavourText("");
                            islastcombomove = true;
                            //Debug.Log("punchcombo");
                        }
                    }
                    if (islastcombomove) audiomanager.Combo();
                    damagedealt += punchbasedamage;
                    guiManager.UpdateDamageText(damagedealt); //if its a new combo give a bonus
                    break;

                case '1': //kick
                    foreach (string weakness in enemyInfo.weaknesses)//determining if there is a new combo
                    {
                        if (prevcomboinfo == null) break;
                        if (comboinfo[weakness] > prevcomboinfo[weakness])//combo acheived
                        {
                            damagedealt += (kickcomboscaling * weakness.Length); //combo effect
                            timedTurnLength += 3 * weakness.Length; // kick adds more time
                            guiManager.UpdateFlavourText("Kick Combo\n" + ComboNumToString(weakness) +"\nExtra Time!");
                            yield return new WaitForSeconds(1f);
                            guiManager.UpdateFlavourText("");
                            islastcombomove = true;

                            //Debug.Log("kickcombo");
                        }
                    }
                    if (islastcombomove) audiomanager.Combo();
                    damagedealt += kickbasedamage;
                    guiManager.UpdateDamageText(damagedealt); //if its a new combo give a bonus
                    break;

                case '2': //parry
                    foreach (string weakness in enemyInfo.weaknesses)//determining if there is a new combo
                    {
                        if (prevcomboinfo == null) break;
                        if (comboinfo[weakness] > prevcomboinfo[weakness])//combo acheived
                        {
                            damagedealt += (parrycomboscaling * weakness.Length); //combo effect
                            parrycount += 1; //parry combo prevents 1 attack from the enemy
                            guiManager.UpdateFlavourText("Parry Combo\n"+ ComboNumToString(weakness) + "\nParry Next Attack!");
                            yield return new WaitForSeconds(1f);
                            guiManager.UpdateFlavourText("");
                            islastcombomove = true;
                            //Debug.Log("parrycombo");
                        }
                    }

                    if (islastcombomove) audiomanager.Combo();

                    damagedealt += parrybasedamage;
                    guiManager.UpdateDamageText(damagedealt); //if its a new combo give a bonus
                    break;

                case '3': //taunt
                    foreach (string weakness in enemyInfo.weaknesses)//determining if there is a new combo
                    {
                        if (prevcomboinfo == null) break;
                        if (comboinfo[weakness] > prevcomboinfo[weakness])//combo acheived
                        {
                            damagedealt += (tauntcomboscaling * weakness.Length); //combo effect
                            playerHp += 4 * weakness.Length; //taunt combo provides heal effect
                            guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);

                            guiManager.UpdateFlavourText("Taunt Combo\n" + ComboNumToString(weakness) +"\nHeal!");
                            yield return new WaitForSeconds(1f);
                            guiManager.UpdateFlavourText("");
                            islastcombomove = true; 

                            //Debug.Log("tauntcombo");
                        }
                    }

                    if (islastcombomove) audiomanager.Combo();
                    damagedealt += tauntbasedamage;
                    guiManager.UpdateDamageText(damagedealt); //if its a new combo give a bonus
                    break;

            }

            if (enemyInfo.IsFinalCombo(potentialcombo))
            {
                enemyInfo.DealDamage(50);
                guiManager.UpdateFlavourText("Ultimate Combo\n EXTREMEDAMAGE");
                guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
                guiManager.UpdateDamageText(damagedealt + 50);
                audiomanager.BigCombo();
                yield return new WaitForSeconds(1f);
                guiManager.UpdateFlavourText("");
                //final combo flavour
                prevcomboinfo = comboinfo;
            }

            else
            {

                prevcomboinfo = comboinfo;
                guiManager.HighlightEnemyHealth(true);

                yield return new WaitForSeconds(0.1f);
                enemyInfo.DealDamage(damagedealt);
                guiManager.HighlightEnemyHealth(false);
                guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
                yield return new WaitForSeconds(0.3f);

                guiManager.UpdateDamageText(-1);
                yield return new WaitForSeconds(0.2f);

            }


        }




        if (CheckWin()) StartCoroutine("HandleVictory");
        if (CheckLoss()) StartCoroutine("Handleloss");

        guiManager.UpdateDamageText(-1);

        StartCoroutine(EnemyAttack(parrycount));
    }



    IEnumerator EnemyAttack(int parrycount)
    {
        StopCoroutine("DealDamage");
        for (int i = 0; i < enemyInfo.enemynumattacks; i++)
        {
            yield return new WaitForSeconds(0.7f);
            if (parrycount > 0)
            {
                parrycount -= 1;

                guiManager.HighlightEnemyHealth(true);
                yield return new WaitForSeconds(0.1f);
                enemyInfo.DealDamage(3);
                guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
                guiManager.HighlightEnemyHealth(false);
                guiManager.UpdateFlavourText("Attack Parried");
                yield return new WaitForSeconds(1f);
                guiManager.UpdateFlavourText("");
                //Debug.Log("Attack Parried");

            }


            else
            {
                playerHp -= enemyInfo.enemydamage;
                guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
            }

        }
        
        if (CheckLoss()) StartCoroutine("Handleloss");
       
        if (CheckWin()) StartCoroutine("HandleVictory");

        else StartCoroutine("TimedTurn");

    }

    IEnumerator HandleVictory()
    {
        StopCoroutine("TimedTurn");
        StopCoroutine("EnemyAttack");
        StopCoroutine("DealDamage");
        yield return new WaitForSeconds(0.4f);
        guiManager.EndGame(true);
    }

    IEnumerator HandleLoss()
    {
        StopCoroutine("TimedTurn");
        StopCoroutine("EnemyAttack");
        StopCoroutine("DealDamage");
        yield return new WaitForSeconds(0.4f);
        guiManager.EndGame(false);
    }


    public bool CheckWin()
    {
        if (enemyInfo.enemyHp <= 0) {
            gamestate = (int)Gamestate.Victory;
            return true;
        }
        return false;

    }

    public bool CheckLoss()
    {
        if (playerHp < 0) {
            gamestate = (int)Gamestate.Loss;
            return true;
        }
        return false;
    }




    void PlayerEnd(bool win)
    {
        StopCoroutine("TimedTurn");
        guiManager.EndGame(win);
    }

    /* Updates the text which appears at the top of the screen representing
     * The sequence of moves which a player enters each turn.
     * returns void
     */

    void UpdateAttackSequence()
    {
        string tempText = "Attacks: "; //holds the current moves entered
        for (int i = 0; i < attackSequence.Length; i++)
        {
            if (i > 0)
            {
                tempText += "/";
            }


            string attackName = "";
            switch (attackSequence[i])
            {
                case '0':
                    attackName = "P";
                    break;
                case '1':
                    attackName = "K";
                    break;
                case '2':
                    attackName = "Py";
                    break;
                case '3':
                    attackName = "T";
                    break;
            }
            tempText = tempText + attackName;
            //Debug.Log(tempText);
            //Debug.Log(attackSequence[i]);
            
        }
        guiManager.UpdateAttackSequenceText(tempText);
    }

    void RestorePlayerHealth()
    {
        playerHp = 50;
        guiManager.StartUpdateHealth(enemyInfo.enemyHp, playerHp);
    }
    

    /*-------------------------------------------------------------------------
     * Coroutine for keeping track of the amount of time remaining on the 
     * players turn. responsible for:
     * updating time ui through gui manager
     * counting down
     * ending the players turn 
    -------------------------------------------------------------------------*/

    IEnumerator TimedTurn()
    {
        float tempTime = timedTurnLength;
        while (tempTime > 0)
        {
            guiManager.SetTimedTurn(tempTime);
            tempTime--;
            yield return new WaitForSeconds(1);

        }

        //turn over ui? maybe idk, but it would go here. 
        timedTurnLength = timedTurnDefault; 
        guiManager.SetTimedTurn(0);
        Enemyturn();
    }
}
