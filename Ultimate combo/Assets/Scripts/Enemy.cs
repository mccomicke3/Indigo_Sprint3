using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Enemy : ScriptableObject
{
    public static string[] nameList = { "Tom", "Geoff", "Jack", "Jeremy", "Trevor", "Ryan", "Bob", "Billy", "Kevin", "Jeff", "Steve", "Sarah", "Andrew", "Cindy", "Vincent", "Katie", "Lisa", "Eric", "Brock", "Brocc", "Ash", "Ashley" };
    public string enemyName = "Brocc";
    public int level = 1;
    public int[] reactions = new int[4];
    public Sprite enemyImage = null;
    public Enemy(int level = 0)
    {
        enemyName = nameList[Random.Range(0,nameList.Length)];
        int reactionAmount = 4 + (level);
        reactions = NewReactions(reactionAmount);
    }
    public int[] NewReactions(int length)
    {
        int[] newReactions = new int[length];
        for (int i=0; i < length; i++)
        {
            newReactions[i] = Random.Range(0, 4);
        }
        return newReactions;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
