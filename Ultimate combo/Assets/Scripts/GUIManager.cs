using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public enum BodyPart { Head, Body, Legs }
    [SerializeField]
    SpriteRenderer spriteRef = null, headRef = null, bodyRef = null, legsRef = null;
    [SerializeField]
    Text enemyHealthText = null, attackSequenceText = null, enemyNameText = null;
    [SerializeField]
    Slider enemyHealthBar = null, playerHealthBar = null;

    [Header("Menu")]
    [SerializeField]
    GameObject pauseMenu = null;
    [SerializeField]
    KeyCode pauseKey = KeyCode.Escape;
    Color startColor = Color.white;
    float colorDelay = 0;
    float pHealth = 0, eHealth = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (spriteRef != null) startColor = spriteRef.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            ToggleMenu(pauseMenu);
        }
        if (colorDelay > 0 && colorDelay < Time.time)
        {
            spriteRef.color = startColor;
            colorDelay = 0;
        }
    }
    // GUI Control
    public void SetPlayerMaxHP(float maxHp)
    {
        if (playerHealthBar != null) playerHealthBar.maxValue = maxHp;
    }
    public void SetEnemyInfo(Enemy info)
    {
        if (enemyNameText != null) enemyNameText.text = info.enemyName;
        if (enemyHealthText != null) enemyHealthText.text = "HP: " + info.reactions.Count;
        float enemyHp = info.reactions.Count;
        if (enemyHealthBar != null) enemyHealthBar.maxValue = enemyHp;
    }
    // Scene/Game Control
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleMenu(GameObject menu)
    {
        if (menu != null) menu.SetActive(!menu.activeInHierarchy);
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
    IEnumerator UpdateHealth()
    {
        while (Mathf.Abs(enemyHealthBar.value - eHealth) > 0.01f)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemyHealthBar != null)
            {
                enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, eHealth, 10);
            }
        }
        while (Mathf.Abs(playerHealthBar.value - pHealth) > 0.01f)
        {
            yield return new WaitForSeconds(0.5f);
            if (playerHealthBar != null)
            {
                playerHealthBar.value = Mathf.Lerp(playerHealthBar.value, pHealth, 3);
            }
        }
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
    public void UpdateAttackSequenceText(string attackSequence)
    {
        if (attackSequenceText != null) attackSequenceText.text = attackSequence;
    }
}
