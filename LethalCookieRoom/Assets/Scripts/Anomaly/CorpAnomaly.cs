using UnityEngine;

public class CorpAnomaly : AnomalyStateMachine {
    public GameObject wallpaper;
    public GameObject responseObject;
    public int responseTimesToClick;

    private ResponseControl responseControl;

    private void Start() {
        initStateMachine();
        responseControl = responseObject.GetComponent<ResponseControl>();
        sourceCameraMaterialNum = 3;
        wallpaper.SetActive(false);
        TriggerEvent(AnomalyEvent.QueueAnomaly);
    }

    protected override void onIdleEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"CORP Entering state Idle from event {anomalyEvent}");
    }

    protected override void onIdleExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"CORP Leaving state Idle from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.QueueAnomaly) {
            currentCoroutine = timerTriggerAnomaly(waitForCameraSwitchAway());
            StartCoroutine(currentCoroutine);
        }
    }

    protected override void onQueuedEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"CORP Entering state Queued from event {anomalyEvent}");
    }

    protected override void onQueuedExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"CORP Leaving state Queued from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {
            Debug.Log(" - Penaulty triggered from spamming response");
            sanityControl.decreaseSanity(sanityPenalty);
        }
    }

    protected override void onActiveEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"CORP Entering state Active from event {anomalyEvent}");
        wallpaper.SetActive(true);
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
        warningCoroutine = timerTriggerAlarm();
        StartCoroutine(warningCoroutine);
        responseControl.onAnomalyStart(responseTimesToClick);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"CORP Leaving state Active from event {anomalyEvent}");
        wallpaper.SetActive(false);
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
