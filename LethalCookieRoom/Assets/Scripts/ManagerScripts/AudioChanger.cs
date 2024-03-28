using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioChanger : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void SetVolLevel(float sliderValue)
    {
        if (sliderValue == 0f) {
            // need explicit 0 otherwise slider at 0 will play full volume
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        else {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        }
    }
}
