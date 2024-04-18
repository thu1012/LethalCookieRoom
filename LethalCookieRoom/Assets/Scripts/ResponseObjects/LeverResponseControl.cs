using UnityEngine;
using System.Collections;

public class LeverResponseControl : ResponseControl {
    public override void triggerClickResponse() {
        playAudio(triggerAudio, 1f);
        startAnimation();
        playAudio(completeAudio, 0.2f);
        anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
    }
}
