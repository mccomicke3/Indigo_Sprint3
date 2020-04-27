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
    public List<string> weaknesses;
    public Sprite enemyImage = null, enemyHead = null, enemyBody = null, enemyLegs = null;
    public Enemy() //class constructor
    {
        enemyName = nameList[Random.Range(0, nameList.Length)];
        weaknesses = weaknessGenerator();
    }

    public List<string> weaknessGenerator()
    {//first choose a group of weaknesses from the list of viable ones
        string[] weaknessSet =
            {"222223" ,
             "222232" ,
             "222322" ,
             "223222" ,
             "232222" ,
             "322222" ,
             "22224" ,
             "22242" ,
             "22422" ,
             "24222" ,
             "42222" ,
             "22233" ,
             "22323" ,
             "22332" ,
             "23223" ,
             "23232" ,
             "23322" ,
             "32223" ,
             "32232" ,
             "32322" ,
             "33222" ,//These are the only viable combos for combo len 8
             "2234" ,// and the other restrictions. 
             "2243" ,
             "2324" ,
             "2342" ,
             "2423" ,
             "2432" ,
             "3224" ,
             "3242" ,
             "3422" ,
             "4223" ,
             "4232" ,
             "4322" ,
             "2333" ,
             "3233" ,
             "3323" ,
             "3332" ,
             "244" ,
             "424" ,
             "442" ,
             "334" ,
             "343" ,
             "433" };//chooses one of these lenghts of weaknesses

        string moveLengths = weaknessSet[Random.Range(0, weaknessSet.Length - 1)];
        //Debug.Log("here are the movelengths: " + moveLengths);

        //set up a few variables to use in the generator:
        List<string> FinalWeaknesses = new List<string>();
        char[] moveLengtharray = moveLengths.ToCharArray();
        char previousMove = Random.Range(0, 4).ToString()[0];
        //Debug.Log("this is previousmove initial: " + previousMove);
        string GeneratedWeakness = "";
        //Debug.Log(moveLengtharray);

        foreach (char weaknessLen in moveLengtharray)
        {
            GeneratedWeakness = "";
            GeneratedWeakness = GeneratedWeakness + previousMove;
            GeneratedWeakness = UniqueWeakness(GeneratedWeakness,
                                FinalWeaknesses, weaknessLen);

            //Debug.Log("here is generated weakness: " + GeneratedWeakness);
            FinalWeaknesses.Add(GeneratedWeakness);
            previousMove = GeneratedWeakness[GeneratedWeakness.Length - 1];

            //Debug.Log(GeneratedWeakness);
        }



        return FinalWeaknesses;

    }



    /*-------------------------------------------------------------------------
     * Helper function for weakness generator
     *The purpose of this funciton, is to ensure that each of the weaknesses in
     * the FinalWeaknesses is unique as it is generated. It will attempt to
     * produce a unique weakness, then check if it is in the list of final
     * weaknesses, if that weakness is in the final list already, it recursively
     * calls itself to generate another weakness. This continues unitl a unique
     * weakness is found. 
     * 
    -------------------------------------------------------------------------*/



    string UniqueWeakness(string incompleteWeakness,
        List<string> FinalWeaknesses, char weaknessLen)

    {
        int recursivecount = 0;
        for (int i = 0; i < char.GetNumericValue(weaknessLen) - 1; i++)
        {
            incompleteWeakness = incompleteWeakness + Random.Range(0, 4);

        }
        if (recursivecount > 1000) //just incase recursion is broken
        {
            return "recursion exceeded 1000";
        }
        if (FinalWeaknesses.Contains(incompleteWeakness))
        {
            //Debug.Log("recursion");
            recursivecount += 1;
            incompleteWeakness = UniqueWeakness(incompleteWeakness[0].ToString(),
                FinalWeaknesses, weaknessLen);
        }


        return incompleteWeakness;

    }

     /*
    public List<Weaknesses> ValidateInput(List<Weaknesses> weaknessList, int[] inputCombo)
    {
        List<Weaknesses> valid = new List<Weaknesses>();
        foreach (Weaknesses weakness in weaknessList)
        {
            if (inputCombo.Length < weakness.weaknessSet.Count)
            {
                // unsure if the condition expression is correctly implemented
                for (int i = 0; i< (inputCombo.Length - weakness.weaknessSet.Count + 1); i++)
                {
                    for (int c = 0; c < weakness.weaknessSet.Count; c++)
                    {
                        //????? im lost.
                        //if (inputCombo[i] == weakness.weaknessSet[c])
                    }
                }
            }
        }
        return valid;
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
