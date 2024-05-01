using UnityEngine;
using System.Collections;

public class LeverResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        playAudio(triggerAudio, 1f);
        startAnimation();
        if (anomalyStateMachine != null && anomalyStateMachine.getState()==AnomalyStateMachine.AnomalyState.Active) {
            playAudio(completeAudio, 0.05f);
            anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        }
    }
}
