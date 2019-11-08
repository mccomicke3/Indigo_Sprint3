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
    Text enemyHealthText = null, attackSequenceText = null;

    List<int> attackSequence = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack(int type)
    {
        attackSequence.Add(type);
        UpdateAttackSequence();
    }
    void UpdateAttackSequence()
    {
        string tempText = "Attacks: ";
        for (int i = 0; i < attackSequence.Count;i++)
        {
            tempText += attackSequence[i];
            if (i < attackSequence.Count)
            {
                tempText += "/";
            }
        }
        if (attackSequenceText != null) attackSequenceText.text = tempText;
    }
}
