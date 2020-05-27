using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // AudioSource for music
    private AudioSource music;
    // List of all music clips
    public List<AudioClip> allmusic;
    // List of all sound effect sources
    public List<AudioSource> effectsources;
    // List of all sound effects
    public List<AudioClip> alleffects;
    // Tracking for which audio source the next clip is going to be played on
    public int effectnum;
    // Shows music number for testing
    public int musicnum;

    // Start is called before the first frame update
    void Start()
    {
        effectnum = 0;
        music = GameObject.Find("Music Player").GetComponent<AudioSource>();
        music.clip = allmusic[0];
        music.Play();
    }

    // Used to change the music
    public void MChange(int soundnum)
    {
        musicnum = soundnum;
        music.clip = allmusic[soundnum];
    }

    // Used to play the music
    public void MPlay()
    {
        music.Play();
    }

    // Used to pause the music
    public void MPause()
    {
        music.Pause();
    }

    public void Punch()
    {
        effectsources[effectnum].clip = alleffects[0];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == effectsources.Count) effectnum = 0;
    }

    public void Kick()
    {
        effectsources[effectnum].clip = alleffects[1];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == alleffects.Capacity) effectnum = 0;
    }

    public void Grapple()
    {
        effectsources[effectnum].clip = alleffects[2];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == alleffects.Capacity) effectnum = 0;
    }

    public void Taunt()
    {
        effectsources[effectnum].clip = alleffects[3];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == alleffects.Capacity) effectnum = 0;

    }

    public void Combo()
    {
        effectsources[effectnum].clip = alleffects[1];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == effectsources.Count) effectnum = 0;
    }

    public void BigCombo()
    {
        effectsources[effectnum].clip = alleffects[2];
        effectsources[effectnum].Play();
        effectnum += 1;
        if (effectnum == effectsources.Count) effectnum = 0;
    }


}
