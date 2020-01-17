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
    public List<Weaknesses> weaknesses;
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
    public List<Weaknesses> NewWeaknesses(int comboLength, int weaknessNum)
    {
        List<Weaknesses> weaknessList = new List<Weaknesses>();
        for (int i = 0; i < comboLength;i++)
        {
            //Random.Range(0,4)
            weaknessList.Add(new Weaknesses());
        }
        SplitWeaknessList(weaknessList, weaknessNum);
        return weaknessList;
    }
    public void SplitWeaknessList(List<Weaknesses> list, int weaknesses)
    {
        foreach(Weaknesses w in list)
        {
            while (w.weaknessSet.Count < weaknesses)
            {
                int[] i = { 0, 0 };
                i[0] = Random.Range(0, list.Count - 2);
                i[1] = Random.Range(i[0]+1,list.Count-1);
                if(!((1 < i[1]-i[0])&&(i[1] - i[0] < 5)))
                {
                    Vector2Int tempIndex = new Vector2Int(i[0], i[1]);
                    w.weaknessSet.Add(tempIndex);
                }
            }
        }
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
[System.Serializable]
public class Weaknesses
{
    public List<Vector2Int> weaknessSet = new List<Vector2Int>();
}