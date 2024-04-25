﻿using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

// TODO: this is a temporary fix for rotating the dial, remember to delete or replace this after M3
public class DialResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        if (Time.time - lastClickTime < 1) { return; }
        playAudio(triggerAudio, 0.5f);
        startAnimation();

        if (anomalyStateMachine != null && anomalyStateMachine.getState() == AnomalyStateMachine.AnomalyState.Active) {
            lastClickTime = Time.time;
            timesClicked++;
            Debug.Log(timesClicked + "/" + timesToClick);
            if (timesClicked == timesToClick) {
                Debug.Log("anamolyComplete");
                playAudio(completeAudio, 0.01f);
                anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
                timesClicked = 0;
            }
        }
    }
}
