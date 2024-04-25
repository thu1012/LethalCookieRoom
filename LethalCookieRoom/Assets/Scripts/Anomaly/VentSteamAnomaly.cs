using UnityEngine;

public class VentSteamAnomaly : AnomalyStateMachine {
    public GameObject steam;
    public GameObject responseObject;

    private ResponseControl buttonResponseControl;

    void Start() {
        initStateMachine();
        buttonResponseControl = responseObject.GetComponent<ResponseControl>();
        steam.SetActive(false);
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
        steam.SetActive(true);
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
        warningCoroutine = timerTriggerAlarm();
        StartCoroutine(warningCoroutine);
        buttonResponseControl.onAnomalyStart(1);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        steam.SetActive(false);
        StopCoroutine(currentCoroutine);
        StopCoroutine(warningCoroutine);
        anomalyWarning.setAlarmInactive(warningBitmap);
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {

        } else if (anomalyEvent == AnomalyEvent.TimeoutTriggered) {
            Debug.Log(" - Penaulty triggered from timeout");
            sanityControl.decreaseSanity(sanityPenalty);
        }
        currentCoroutine = timerTriggerAnomaly(waitForCameraSwitchAway());
        StartCoroutine(currentCoroutine);
    }
}
