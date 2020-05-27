using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*-------------------------------------------------------------------------
 * GUIManager
 * Script to make updating the visual elements of the game easier
 * also makes some simple functions for loading scenes and quitting the game'
 * To be a component of 
 *      The EventSystem
 * A reference of the eventsystem is to be attached to the enemy object
 * Responsible for:
 * 
 * handling pause menu
 * updating ui objects
 * loading/reloading/quiting the current scene
 * 
 *
 * 
-------------------------------------------------------------------------*/

public class GUIManager : MonoBehaviour
{
    public enum BodyPart { Head, Body, Legs }
    [SerializeField]
    SpriteRenderer spriteRef = null, headRef = null, bodyRef = null, legsRef = null;
    [SerializeField]
    Text enemyHealthText = null, attackSequenceText = null, enemyNameText = null;
    [SerializeField]
    Text gameOverText = null, timerText = null, damageText = null, knownCombosText = null, flavourText;
    [SerializeField]
    Slider enemyHealthBar = null, playerHealthBar = null; Slider timerSlider;
    [SerializeField]
    Image enemyHpFill;
    [SerializeField]
    GameObject enemyHighlight = null;
    [SerializeField]
    KeyCode pauseKey = KeyCode.Escape;
    [Header("Menu")]
    [SerializeField]
    GameObject pauseMenu = null, loseMenu = null, winMenu = null;


    float pHealth = 0, eHealth = 0;
    float colorDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // GUI Control
    public void SetPlayerMaxHP(float maxHp)
    {
        if (playerHealthBar != null) playerHealthBar.maxValue = maxHp;
    }
    public void SetEnemyInfo(Enemy info)
    {
        if (enemyNameText != null) enemyNameText.text = info.enemyName;
        if (enemyHealthText != null) enemyHealthText.text = "HP: " + info.enemyHp;
        float enemyHp = info.enemyHp;
        if (enemyHealthBar != null) enemyHealthBar.maxValue = enemyHp;
    }
    public void SetTimedTurn(float time)
    {
        if (timerText == null) return;
        timerText.text = "Turn: " + GetTimeText(time);
    }

    public void ShowEnemyHighlight(bool show)
    {
        if (enemyHighlight != null) enemyHighlight.SetActive(show);
    }
    /*-------------------------------------------------------------------------
     * Updates the damage text
     * If it is given a negative value, it will hide the text instead of just
     * displaying it. 
    -------------------------------------------------------------------------*/

    public void UpdateDamageText(int damage)
    {

        if (damageText == null) return;
        if (!damageText.IsActive()){
            damageText.gameObject.SetActive(true);
        }
        if (damage < 0)
        {
            damageText.gameObject.SetActive(false);
            return;
        }
        damageText.text = damage.ToString();
        //damageText.gameObject.SetActive(false);
    }

    public void UpdateKnownComboText(List<string> knownCombo)
    {
        string outstring = "";
        foreach (string weakness in knownCombo) {
            string temp = "";
            foreach(char move in weakness)
            {
                string movename = "";
                switch (move)
                {
                    case '0':
                        movename = "P ";
                        break;
                    case '1':
                        movename = "K ";
                        break;
                    case '2':
                        movename = "Py ";
                        break;
                    case '3':
                        movename = "T ";
                        break;
                    case '*':
                        movename = "* ";
                        break;
                }
                temp = temp + movename;
            }



            outstring = outstring + temp + "\n";
                }
        knownCombosText.text = outstring;
    }

    /*-------------------------------------------------------------------------
     * changes the color on enemy health bar, meant to make it more clear when
     * damage is dealt to the enemy. if true highlights the color, otherwise
     * will set it to its original value. 
    -------------------------------------------------------------------------*/
    public void HighlightEnemyHealth(bool status)
    {
        if (status) {
            enemyHpFill.color = new Color(255, 150, 150);
        }
        else
        {
            enemyHpFill.color = new Color(255, 0, 0);
        }
    }


    // Scene/Game Control
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleMenu()
    {
        if (pauseMenu != null) pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }
    public void SetSpriteColor(Color spriteColor)
    {
        if (spriteRef == null) return;
        spriteRef.color = spriteColor;
        colorDelay = Time.time + 1;
    }
    public void StartUpdateHealth(float enemyHealth, float playerHealth)
    {
        eHealth = enemyHealth;
        pHealth = playerHealth;
        StartCoroutine("UpdateHealth");
    }


    /*-------------------------------------------------------------------------
    Update health coroutine, which sets the values of the healthbars to be 
    a particular value after a set amount of time. 
    -------------------------------------------------------------------------*/


    IEnumerator UpdateHealth()
    {
        while (Mathf.Abs(enemyHealthBar.value - eHealth) > 0.01f)
        {
            yield return new WaitForSeconds(0.05f);
            if (enemyHealthBar != null)
            {
                enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, eHealth, 0.8f);
            }
            
        }
        enemyHealthBar.value = eHealth;

        while (Mathf.Abs(playerHealthBar.value - pHealth) > 0.01f)
        {
            yield return new WaitForSeconds(0.1f);
            if (playerHealthBar != null)
            {
                playerHealthBar.value = Mathf.Lerp(playerHealthBar.value, pHealth, 0.8f);
            }
        }
        playerHealthBar.value = pHealth;
    }
    public void EnemyBodySprite(BodyPart part, Sprite sprite)
    {
        switch (part)
        {
            case BodyPart.Head:
                if (headRef != null) headRef.sprite = sprite;
                break;
            case BodyPart.Body:
                if (bodyRef != null) bodyRef.sprite = sprite;
                break;
            case BodyPart.Legs:
                if (legsRef != null) legsRef.sprite = sprite;
                break;
        }
    }

    public void UpdateFlavourText(string newtext)
    {
        if (newtext == "") flavourText.gameObject.SetActive(false);
        else
        {
            flavourText.gameObject.SetActive(true);
            flavourText.text = newtext;
        }

    }

    public void UpdateAttackSequenceText(string attackSequence)
    {
        if (attackSequenceText != null) attackSequenceText.text = attackSequence;
    }
    public void EndGame(bool win)
    {
        if (win)
        {
            winMenu.SetActive(true);
        }
        loseMenu.SetActive(true);
    }
    string GetTimeText(float time)
    {
        return string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
    }
}
