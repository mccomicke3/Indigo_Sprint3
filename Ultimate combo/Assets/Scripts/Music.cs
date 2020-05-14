using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource source;
    public AudioClip current;
    public int currentnum;
    public List<AudioClip> allmusic;

    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("Audio Source"). GetComponent<AudioSource>();
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Used to change the music
    void MChange(int soundnum)
    {
        currentnum = soundnum;
        current = allmusic[soundnum];
    }

    // Used to play the music
    void MPlay()
    {
        source.Play();
    }

    // Used to pause the music
    void MPause()
    {
        source.Pause();
    }
}
