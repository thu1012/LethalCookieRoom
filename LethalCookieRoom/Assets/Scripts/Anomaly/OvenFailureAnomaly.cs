using UnityEngine;

public class OvenFailureAnomaly : AnomalyStateMachine {
    public GameObject oven;
    public ButtonResponseControl buttonResponseControl;
    public GameObject responseObject;
    void Start() {
        initStateMachine(40, 20, 1);
        TriggerEvent(AnomalyEvent.QueueAnomaly);
        sanityControl = GameObject.Find("/Player").GetComponent<SanityControl>();
        buttonResponseControl = responseObject.GetComponent<ButtonResponseControl>();
    }

    protected override void onIdleEnter(AnomalyEvent anomalyEvent) {
        oven.SetActive(false);
        Debug.Log($"Entering state Idle from event {anomalyEvent}");
    }

    protected override void onIdleExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Idle from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.QueueAnomaly) {
            currentCoroutine = timerTriggerAnomaly();
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
        oven.SetActive(true);
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
        buttonResponseControl.onAnomalyStart(1);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        oven.SetActive(false);
        StopCoroutine(currentCoroutine);
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {

        } else if (anomalyEvent == AnomalyEvent.TimeoutTriggered) {
            Debug.Log(" - Penaulty triggered from timeout");
            sanityControl.decreaseSanity(sanityPenalty);
        }
        currentCoroutine = timerTriggerAnomaly();
        StartCoroutine(currentCoroutine);
    }
}
