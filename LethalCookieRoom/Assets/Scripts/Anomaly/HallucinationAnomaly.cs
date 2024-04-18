using UnityEngine;
using System.Collections;
using Random = System.Random;

public class HallucinationAnomaly : AnomalyStateMachine {
    public GameObject shadowFigure;

    private bool anomalyReady = false;

    void Start() {
        initStateMachine();
        shadowFigure.SetActive(false);
        TriggerEvent(AnomalyEvent.QueueAnomaly);
    }

    protected override void onIdleEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"Entering state Idle from event {anomalyEvent}");
    }

    protected override void onIdleExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Idle from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.QueueAnomaly) {
            currentCoroutine = timerTriggerReadyAnomaly();
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
        shadowFigure.SetActive(true);
        anomalyReady = false;
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        shadowFigure.SetActive(false);
        StopCoroutine(currentCoroutine);
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {

        } else if (anomalyEvent == AnomalyEvent.TimeoutTriggered) {
            Debug.Log(" - Penaulty triggered from timeout");
            sanityControl.decreaseSanity(sanityPenalty);
        }
        currentCoroutine = timerTriggerReadyAnomaly();
        StartCoroutine(currentCoroutine);
    }

    public bool getAnomalyReady() {
        return anomalyReady;
    }

    private IEnumerator timerTriggerReadyAnomaly() {
        //Debug.LogFormat($"Triggering anomaly in {time} seconds");
        yield return new WaitForSecondsRealtime(anomalyTriggerSeconds);

        Random random = new Random();
        if (random.NextDouble() < anomalyTriggerProbability) {
            anomalyReady = true;
        } else {
            currentCoroutine = timerTriggerReadyAnomaly();
            StartCoroutine(currentCoroutine);
        }
    }
}
