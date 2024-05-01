using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyWarning : MonoBehaviour {
    public AudioClip audioClip;
    public float alarmVolume;
    public List<GameObject> anomalyObjects;

    private int alarmBitmap = 0;
    private int alarmLevel = 0;
    private AudioSource audioSource;

    void Start() {
        initBitmapValue();
        initAlarm();
    }

    public void setAlarmActive(int bitmap) {
        alarmBitmap = alarmBitmap | bitmap;
        updateAlarmLevel();
    }

    public void setAlarmInactive(int bitmap) {
        alarmBitmap = alarmBitmap & (~bitmap);
        updateAlarmLevel();
    }

    private void updateAlarmLevel() {
        int level = 0;
        int alarmBitmapCopy = alarmBitmap;
        while (alarmBitmapCopy > 0) {
            if (alarmBitmapCopy % 2 == 1) {
                level++;
            }
            alarmBitmapCopy /= 2;
        }
        alarmLevel = level;
    }

    private void initBitmapValue() {
        int bitmap = 1;
        for (int i = 0; i < anomalyObjects.Count; i++) {
            anomalyObjects[i].GetComponent<AnomalyStateMachine>().setWarningBitmap(bitmap);
            bitmap *= 2;
        }
    }

    private void initAlarm() {
        if (audioClip != null) {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null) {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = audioClip;
            audioSource.volume = alarmVolume;
            playAlarm();
        }
    }

    private void playAlarm() {
        if (alarmLevel == 0) {
            Invoke("playAlarm", 0.5f);
        } else {
            audioSource.Play();
            Invoke("playAlarm", 2f / alarmLevel);
        }
    }
}
