using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Enemy : ScriptableObject
{
    public static string[] maleNames = { "Tom", "Geoff", "Jack", "Jeremy", "Trevor", "Ryan", "Bob", "Billy", "Kevin", "Jeff", "Steve", "Andrew", "Vincent", "Eric", "Brock", "Brocc", "Ash" },
        femaleNames = { "Sarah", "Cindy", "Katie", "Lisa", "Ashley" };
    public string[] nameList = maleNames;
    public string enemyName = "Brocc";
    public List<Reactions> reactions;
    public Sprite enemyImage = null, enemyHead = null, enemyBody = null, enemyLegs = null;
    public int[,] testReactions;
    public Enemy(int level = 0)
    {
        enemyName = nameList[Random.Range(0,nameList.Length)];
        int reactionAmount = 4 + (level);
        reactions = NewReactions(reactionAmount);
    }
    public List<Reactions> NewReactions(int length)
    {
        List<Reactions> newReactions = new List<Reactions>();
        for (int i=0; i < length; i++)
        {
            newReactions.Add(new Reactions());
            int randReactions = Random.Range(1, 3);
            for (int r = 0; r < randReactions; r++)
            {
                newReactions[i].reactionSet.Add(Random.Range(0, 4));
            }
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
[System.Serializable]
public class Reactions
{
    public List<int> reactionSet = new List<int>();
}
