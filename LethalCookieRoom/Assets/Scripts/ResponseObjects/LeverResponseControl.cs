using UnityEngine;
using System.Collections;

// TODO: this is a temporary fix for rotating the dial, remember to delete or replace this after M3
public class LeverResponseControl : ResponseControl {

    private void Start() {
        onAnomalyStart(1);
    }

    public override void triggerClickResponse() {
        playAudio(triggerAudio, 1f);
        startAnimation();
        playAudio(completeAudio, 0.2f);
        anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
    }
}
