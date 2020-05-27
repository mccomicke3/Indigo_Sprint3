using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Animator fadetoblack;

    void Start()
    {
        
    } 
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Fade());
        }

    }

    IEnumerator Fade()
    {
        fadetoblack.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MenuScene");
    }
   
    void SceneChange()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("Fade");
        }
    
    }

    


    public void StartGame()
    {
        SceneManager.LoadScene("BaseScene");
    }
}
