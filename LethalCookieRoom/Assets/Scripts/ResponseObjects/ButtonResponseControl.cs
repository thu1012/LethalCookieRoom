using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class ButtonResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        if (Time.time - lastClickTime < 1) { timesClicked = 0; }
        playAudio(triggerAudio, 1f);
        startAnimation();
        lastClickTime = Time.time;
        timesClicked++;
        Debug.Log(timesClicked + "/" + timesToClick);
        if (timesClicked == timesToClick) {
            Debug.Log("anamoly complete");
            playAudio(completeAudio, 0.2f);
            anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
            timesClicked = 0;
        }
    }
}
