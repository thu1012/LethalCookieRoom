using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class DialResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        if (Time.time - lastClickTime < 1) { return; }
        playAudio(triggerAudio, 1f);
        startAnimation();

        if (anomalyStateMachine != null && anomalyStateMachine.getState() == AnomalyStateMachine.AnomalyState.Active) {
            lastClickTime = Time.time;
            timesClicked++;
            Debug.Log(timesClicked + "/" + timesToClick);
            if (timesClicked == timesToClick) {
                Debug.Log("anamoly complete");
                playAudio(completeAudio, 0.05f);
                anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
                timesClicked = 0;
            }
        }
    }
}
