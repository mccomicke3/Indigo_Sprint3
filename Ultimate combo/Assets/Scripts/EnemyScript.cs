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

    float testDelay = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((test && Input.GetKey(testKey)) && testDelay < Time.time)
        {
            enemyInfo = new Enemy(Random.Range(0,5));
            if (enemyNameText != null) enemyNameText.text = enemyInfo.enemyName;
            if (enemyHealthText != null) enemyHealthText.text = "HP: " + enemyInfo.reactions.Length;
            testDelay = Time.time + 0.5f;
        }
    }
    public void AddCombo(int type)
    {
        if (type == prevAttack) return;
        attackSequence.Add(type);
        UpdateAttackSequence();
        prevAttack = type;
    }
    public void ClearSequence()
    {
        attackSequence.Clear();
        UpdateAttackSequence();
        prevAttack = -1;
    }
    public void Attack()
    {
        Debug.Log("You attacked");
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
