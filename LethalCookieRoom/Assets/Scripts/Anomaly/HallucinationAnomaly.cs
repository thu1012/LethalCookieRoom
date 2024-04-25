using UnityEngine;
using System.Collections;
using Random = System.Random;

public class HallucinationAnomaly : AnomalyStateMachine {
    public GameObject shadowFigure;
    public int roomsCount;

    private Random random;

    void Start() {
        initStateMachine();
        random = new Random();
        shadowFigure.SetActive(false);
        TriggerEvent(AnomalyEvent.QueueAnomaly);
    }

    protected override void onIdleEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"Entering state Idle from event {anomalyEvent}");
    }

    protected override void onIdleExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Idle from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.QueueAnomaly) {
            currentCoroutine = timerTriggerAnomaly(waitForCameraSwitchTo());
            StartCoroutine(currentCoroutine);
        }
    }

    protected override void onQueuedEnter(AnomalyEvent anomalyEvent) {
        Debug.Log($"Entering state Queued from event {anomalyEvent}");
        sourceCameraMaterialNum = random.Next(roomsCount);
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
        currentCoroutine = timerTriggerTimeout();
        StartCoroutine(currentCoroutine);
        warningCoroutine = timerTriggerAlarm();
        StartCoroutine(warningCoroutine);
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        shadowFigure.SetActive(false);
        StopCoroutine(currentCoroutine);
        StopCoroutine(warningCoroutine);
        anomalyWarning.setAlarmInactive(warningBitmap);
        if (anomalyEvent == AnomalyEvent.ResponseTriggered) {

        } else if (anomalyEvent == AnomalyEvent.TimeoutTriggered) {
            Debug.Log(" - Penaulty triggered from timeout");
            sanityControl.decreaseSanity(sanityPenalty);
        }
        currentCoroutine = timerTriggerAnomaly(waitForCameraSwitchTo());
        StartCoroutine(currentCoroutine);
    }

    protected IEnumerator waitForCameraSwitchTo()
    {
        yield return new WaitForEndOfFrame();

        if (screenControl == null) {
            screenControl = GameObject.Find("Office/monitor").GetComponent<ScreenControl>();
        }
        if (screenControl.getCameraMaterial() != sourceCameraMaterialNum) {
            TriggerEvent(AnomalyEvent.TriggerAnomaly);
        } else {
            StartCoroutine(waitForCameraSwitchTo());
        }
    }
}
