using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioChanger : MonoBehaviour
{

    public static AudioChanger Instance {get; private set;}
    public AudioMixer audioMixer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }


    void Start()
    {

    }

    public void SetVolLevel(float sliderValue)
    {
        if (sliderValue == 0f) {
            // need explicit 0 otherwise slider at 0 will play full volume
            PlayerPrefs.GetFloat("AudioLevel", -80f);
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        else {
            PlayerPrefs.GetFloat("AudioLevel", Mathf.Log10(sliderValue) * 20);
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        }
        
    }
}
