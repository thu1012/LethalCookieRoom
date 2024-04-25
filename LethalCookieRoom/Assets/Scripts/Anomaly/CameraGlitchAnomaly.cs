using UnityEngine;

public class CameraGlitchAnomaly : AnomalyStateMachine {
    public GameObject screenObject;
    private ScreenControl screenControl;

    void Start() {
        screenControl = screenObject.GetComponent<ScreenControl>();
        initStateMachine();
        TriggerEvent(AnomalyEvent.QueueAnomaly);
    }

    protected override void onIdleEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"Entering state Idle from event {anomalyEvent}");
    }

    protected override void onIdleExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Idle from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.QueueAnomaly) {
            currentCoroutine = timerTriggerAnomaly(waitForCameraSwitchAway());
            StartCoroutine(currentCoroutine);
        }
    }

    protected override void onQueuedEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"Entering state Queued from event {anomalyEvent}");
    }

    protected override void onQueuedExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Queued from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {
            Debug.Log(" - Penaulty triggered from spamming response");
            sanityControl.decreaseSanity(sanityPenalty);
        }
    }

    protected override void onActiveEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"Entering state Active from event {anomalyEvent}");
        screenControl.triggerGlitchAnomaly();
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
        warningCoroutine = timerTriggerAlarm();
        StartCoroutine(warningCoroutine);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        StopCoroutine(currentCoroutine);
        StopCoroutine(warningCoroutine);
        anomalyWarning.updateAlarmLevel(0);
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {
            screenControl.resolveGlitchAnomaly();
        } else if (anomalyEvent == AnomalyEvent.TimeoutTriggered) {
            Debug.Log(" - Penaulty triggered from timeout");
            sanityControl.decreaseSanity(sanityPenalty);
        }
        currentCoroutine = timerTriggerAnomaly(waitForCameraSwitchAway());
        StartCoroutine(currentCoroutine);
    }
}
