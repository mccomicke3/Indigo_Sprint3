using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

/*-------------------------------------------------------------------------
 * Enemy Class
 * Holds some information about the enemy entity, including:
 * a name string
 * a list of the combos which the enemy is weak to
 * sprites which represent the enemy
 * an integer representing its HP
 * 
 * Also has several methods,
 * IsFinalCombo(Input list) checks the win state. 
 * WeaknessGenerator() will generate a new random list of weaknesses
 * 
-------------------------------------------------------------------------*/
public class Enemy : ScriptableObject
{
    public int enemyHp = 100;
    public string[] nameList = { "Tom", "Geoff", "Jack", "Jeremy", "Trevor", "Ryan", "Bob", "Billy", "Kevin", "Jeff", "Steve", "Andrew", "Vincent", "Eric", "Brock", "Brocc", "Ash" };
    public string enemyName = "Brocc";
    public List<string> weaknesses = null;
    public Sprite enemyImage = null, enemyHead = null, enemyBody = null, enemyLegs = null;

    public Enemy() //class constructor
    {

    }


    /*-------------------------------------------------------------------------
 * Weakness generator
 * The purpose of this method, is to produce a list of strings which represent
 * the weaknesses an Enemy has, this list of strings follows these criteria
 *  - consist of n moves between (2, 4)
    - there are 4 possible moves and in this code will be represented by integers
      0, 1, 2, 3
    - each concequtive weakness begins with the last entry of the first weakness
    - each weakness must be unique
    - each combo must meld together and fit into the ultimate combo in 8 moves.
    concequentially there are a finite number of combonations which may exist,
    
    the list weaknessSet is the list of viable lengths of the weaknesses which
    may exist, the algorithm procedurally builds the combos by picking the last
    move of each combo to be the first move of the next, retrying if it is not
    unique.
 * 
-------------------------------------------------------------------------*/

    public List<string> WeaknessGenerator()
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


    /*-------------------------------------------------------------------------
     * randomizes the weaknesses and name of the Enemy. 
     -------------------------------------------------------------------------*/
    public void Randomize()
    {
        enemyName = nameList[Random.Range(0, nameList.Length)];
        weaknesses = WeaknessGenerator();

    }

    /*-------------------------------------------------------------------------
     * returns a integer equal to the number of weaknesses which the inputted
     * string contains. returns 0 if there are no weaknesses within the 
     * inputted values.
    -------------------------------------------------------------------------*/


    public int IsCombo(string userinput)
    {
        int successnum = 0;
        foreach (string weakness in weaknesses)
        {
            if (userinput.Contains(weakness)) successnum += 1;
            //Debug.Log("searching for :" + weakness);
        }
        return successnum;
    }
    /*-------------------------------------------------------------------------
     * returns true or false depending on whether the input value contains
     * all of the enemy weaknesses 
    -------------------------------------------------------------------------*/
    public bool IsFinalCombo(string userinput)
    {
        if (IsCombo(userinput) == weaknesses.Count) return true;
        return false;
    }

    /*-------------------------------------------------------------------------
     * updates the data held in the known weaknesses baised on the input moves
     * of the user and the currently known weaknesses of the user. 
     * - * represents an unknown move in the sequence
     * - 0, 1, 2, 3 represent known moves in the sequence
     * returns the updated list of strings known to the user.
    -------------------------------------------------------------------------*/
    public List<string> UpdateKnownWeaknesses(List<string> knownweaknesses, string userinput)
    {
        
        if (knownweaknesses.Count == 0) return CensoredWeaknesses();
        List<string> updatedlist = new List<string>();

        for(int weaknessindex = 0 ; weaknessindex < weaknesses.Count; weaknessindex++)
        {
            string updatedweakness = UpdateWeakness(userinput, knownweaknesses[weaknessindex],
               weaknesses[weaknessindex]);
            updatedlist.Add(updatedweakness);
        }

        return updatedlist;
    }

    /*-------------------------------------------------------------------------
    returns a list of censored weaknesses, that is a list of strings which are
    simply asterisks in the same order and number as the order and number
    of weaknesses which the enemy has.
    -------------------------------------------------------------------------*/
    public List<string> CensoredWeaknesses()
    {
        List<string> outlist = new List<string>();
        foreach (string weakness in weaknesses)
        {
            string outstring = "";
            foreach (char move in weakness)
            {
                outstring += "*";
            }
            outlist.Add(outstring);
        }
        return outlist;
    }

    /*-------------------------------------------------------------------------
     * helper function for UpdateKnownWeakness
     * updates the data held in an individual string, being of the form
     * of asterisks and numbers depending on how much information the player
     * has availible to them at any given moment.
     * ie. "01**" or "****" or "0123"
     * parameters are:
     * userinput ie. 10232100
     * knownweakness ie. 00**
     * actualweakness ie. 0022
     * returns the updated string.
    -------------------------------------------------------------------------*/


    string UpdateWeakness(string userinput, string knownweakness, string actualweakness)
        
    {
        if (userinput.Contains(actualweakness)) return actualweakness;
        if (knownweakness == actualweakness) return actualweakness;
        string teststring = "";
        int success = 0;
        for (int i = 0; i < actualweakness.Length; i++) 
        { // searches and sees if there are newly discovered moves
            teststring = teststring + actualweakness[i];
            if (userinput.Contains(teststring))
            {
                success++;
            }
            else break;

        }
        if (success == 0) //If no new moves are discovered
        {
            return knownweakness;
        }
        else //if new moves are discovered
        {
            knownweakness = "";

            for (int i = 0; i < success; i++)
            {
                knownweakness = knownweakness + actualweakness[i];
            }

            for (int i = actualweakness.Length - success; i > 0; i--)
            {
                knownweakness = knownweakness + "*";
            }

            return knownweakness;
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