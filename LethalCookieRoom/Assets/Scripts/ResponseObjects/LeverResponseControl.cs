using UnityEngine;
using System.Collections;

// TODO: this is a temporary fix for rotating the dial, remember to delete or replace this after M3
public class LeverResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        playAudio(triggerAudio, 0.5f);
        startAnimation();
        if (anomalyStateMachine != null && anomalyStateMachine.getState()==AnomalyStateMachine.AnomalyState.Active) {
            playAudio(completeAudio, 0.05f);
            anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        }
    }
}
