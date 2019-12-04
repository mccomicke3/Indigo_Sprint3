using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour {
    
    static float volumeValue = -30f;

    public AudioMixer audioMixer;
    [Tooltip("Music Slider has to have whole numbers from 0-10, starting at max value")]
    public Slider volumeSlider;

    private AudioSource audioSource;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        if (volumeValue != -30f)
        {
            volumeSlider.value = volumeValue;
            audioMixer.SetFloat("MasterVolume", ConvertAudioValue(volumeSlider.value));
        }        
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("MasterVolume", ConvertAudioValue(volumeSlider.value));
    }

    float ConvertAudioValue(float sliderValue)
    {
        float audioValue = (sliderValue * 5) - 50;
        if (sliderValue < 1)
        {
            audioValue = -80;
        }
        return audioValue;
    }

    public void ToggleAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
}
