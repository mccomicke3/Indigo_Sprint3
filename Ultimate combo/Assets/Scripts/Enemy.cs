using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Enemy : ScriptableObject
{
    public static string[] nameList = { "Tom", "Geoff", "Jack", "Jeremy", "Trevor", "Ryan", "Bob", "Billy", "Kevin", "Jeff", "Steve", "Sarah", "Andrew", "Cindy", "Vincent", "Katie", "Lisa", "Eric", "Brock", "Brocc", "Ash", "Ashley" };
    public string enemyName = "Brocc";
    public int level = 1;
    public List<Reactions> reactions;
    public Sprite enemyImage = null;
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
            int randReactions = Random.Range(1, 4);
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
