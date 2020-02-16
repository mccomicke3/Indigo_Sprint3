using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessGen : MonoBehaviour {

    // Start is called before the first frame update
    void Start()
    {
        weaknessgeneratortest();
        //testuniqueweakness();
    }

    // Update is called once per frame
    void Update()
    {

    }




    /* This generates a list of unique weaknesses which pulls from a static list
     * of possible weaknesslengths, generates a unique series of weaknesses,
     * which will be able to be fit into a 8 move combo, with exactly 1 overlap
     * between each move. The weaknesses follow the following behaviours:
     * - consist of n moves between (2, 4)
     * - there are 4 possible moves represented by numbers (0, 1, 2, 3)
     * - each weakness is unique
     * - combo must meld together and fit into a minimum of 8 values
     *
     *
     *
     * Returns a list of strings which represents the weaknesses
     */
   
    public List <string> weaknessgenerator()
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

        string moveLengths = weaknessSet[Random.Range(0, weaknessSet.Length-1)];
        //Debug.Log("here are the movelengths: " + moveLengths);

        //set up a few variables to use in the generator:
        List<string> finalWeaknesses = new List<string>();
        char[] moveLengtharray = moveLengths.ToCharArray();
        char previousMove = Random.Range(0, 4).ToString()[0];
        //Debug.Log("this is previousmove initial: " + previousMove);
        string generatedWeakness = "";
        //Debug.Log(moveLengtharray);

        foreach (char weaknessLen in moveLengtharray)
        {

            generatedWeakness = "";
            generatedWeakness = generatedWeakness + previousMove;
            generatedWeakness = uniqueWeakness(generatedWeakness,
                                finalWeaknesses, weaknessLen);

            //Debug.Log("here is generated weakness: " + generatedWeakness);
            finalWeaknesses.Add(generatedWeakness);
            previousMove = generatedWeakness[generatedWeakness.Length-1];

            
            //Debug.Log(generatedWeakness);



        }



        return finalWeaknesses ;

    }



    /*-------------------------------------------------------------------------
     * Helper function for weakness generator
     *The purpose of this funciton, is to ensure that each of the weaknesses in
     * the finalweaknesses is unique as it is generated. It will attempt to
     * produce a unique weakness, then check if it is in the list of final
     * weaknesses, if that weakness is in the final list already, it recursively
     * calls itself to generate another weakness. This continues unitl a unique
     * weakness is found. 
     * 
    -------------------------------------------------------------------------*/



    public string uniqueWeakness(string incompleteWeakness,
        List<string> finalWeaknesses, char weaknessLen)

    {
        int recursivecount = 0;
        for (int i = 0; i < char.GetNumericValue(weaknessLen)-1 ; i++)
        {
            incompleteWeakness = incompleteWeakness + Random.Range(0, 4);
            
        }
        if (recursivecount > 1000) //just incase recursion is broken
        {
            return "recursion exceeded 1000";
        }
        if (finalWeaknesses.Contains(incompleteWeakness)){
            //Debug.Log("recursion");
            recursivecount += 1;
            incompleteWeakness = uniqueWeakness(incompleteWeakness[0].ToString(),
                finalWeaknesses, weaknessLen);
        }
       

        return incompleteWeakness;

    }


    //function to test uniqueweakness generator
     void testuniqueweakness()
    {
        for (int i = 0; i < 10; i++)
        {
            List<string> testList = new List<string>();
            testList.Add("131");
            string incompleteTest = "1";

            string testWeakness = uniqueWeakness(incompleteTest, testList, '3');
            Debug.Log(testWeakness);
        }
    }





    void weaknessgeneratortest()
    {
        List<string> generatedweaknesses = weaknessgenerator();
        foreach (string weakness in generatedweaknesses)
        {
            Debug.Log(weakness);
        }

    }



}
