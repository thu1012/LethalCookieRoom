using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class ButtonResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        if (Time.time - lastClickTime < 1) { return; }
        else if (Time.time - lastClickTime < 1) { timesClicked = 0; }
        playAudio(triggerAudio, 0.5f);
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
