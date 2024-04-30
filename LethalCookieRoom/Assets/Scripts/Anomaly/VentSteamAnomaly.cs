using UnityEngine;

public class VentSteamAnomaly : AnomalyStateMachine {
    public GameObject steam;
    public GameObject responseObject;
    public float responseTimeToHold;

    private ResponseControl responseControl;

    void Start() {
        initStateMachine();
        responseControl = responseObject.GetComponent<ResponseControl>();
        sourceCameraMaterialNum = 0;
        setSteamEmissionRate(0f);
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
        setSteamEmissionRate(20f);
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
        warningCoroutine = timerTriggerAlarm();
        StartCoroutine(warningCoroutine);
        responseControl.onAnomalyStart(responseTimeToHold);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        setSteamEmissionRate(0f);
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

    private void setSteamEmissionRate(float rate)
    {
        foreach(Transform child in steam.transform)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            var emissionModule = ps.emission;
            emissionModule.rateOverTime = rate;
        }
    }
}
