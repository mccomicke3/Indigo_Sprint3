using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // AudioSource for music
    private AudioSource music;
    // Shows music number for testing
    public int currentnum;
    // List of all music clips
    public List<AudioClip> allmusic;
    // List of all sound effect sources
    public List<AudioSource> effectsources;
    // List of all sound effects
    public List<AudioClip> alleffects;
    // Tracking for which audio source the next clip is going to be played on
    public int effectnum;

    // Start is called before the first frame update
    void Start()
    {
        effectnum = 0;
        music = GameObject.Find("Music Player").GetComponent<AudioSource>();
        music.Play();
    }

    // Used to change the music
    void MChange(int soundnum)
    {
        currentnum = soundnum;
        music.clip = allmusic[soundnum];
    }

    // Used to play the music
    void MPlay()
    {
        music.Play();
    }

    // Used to pause the music
    void MPause()
    {
        music.Pause();
    }

    void Punch()
    {
        effectsources[effectnum].clip = alleffects[0];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == 9) effectnum = 0;
    }

    void Kick()
    {
        effectsources[effectnum].clip = alleffects[1];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == 9) effectnum = 0;
    }

    void Grapple()
    {
        effectsources[effectnum].clip = alleffects[2];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == 9) effectnum = 0;
    }

    void Taunt()
    {
        effectsources[effectnum].clip = alleffects[3];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == 9) effectnum = 0;
    }
}
