using UnityEngine;

public class TestAnomaly : AnomalyStateMachine
{
    void Start()
    {
        initStateMachine(5, 10, 0.6);
        TriggerEvent(AnomalyEvent.ResponseTriggered);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Triggering response event");
            TriggerEvent(AnomalyEvent.ResponseTriggered);
        }
    }

    protected override void onIdleEnter(AnomalyEvent anomalyEvent)
    {
        Debug.Log($"Entering state Idle from event {anomalyEvent}");
    }

    protected override void onIdleExit(AnomalyEvent anomalyEvent)
    {
        Debug.Log($"Leaving state Idle from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.QueueAnomaly)
        {
            StartCoroutine(TimerTriggerAnomaly());
        }
    }

    protected override void onQueuedEnter(AnomalyEvent anomalyEvent)
    {
        Debug.Log($"Entering state Queued from event {anomalyEvent}");
    }

    protected override void onQueuedExit(AnomalyEvent anomalyEvent)
    {
        Debug.Log($"Leaving state Queued from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.ResponseTriggered)
        {
            Debug.Log(" - Penaulty triggered from spamming response");
        }
    }

    protected override void onActiveEnter(AnomalyEvent anomalyEvent)
    {
        Debug.Log($"Entering state Active from event {anomalyEvent}");
        StartCoroutine(TimerTriggerTimeout());
    }

    protected override void onActiveExit(AnomalyEvent anomalyEvent)
    {
        Debug.Log($"Leaving state Active from event {anomalyEvent}");
        if (anomalyEvent == AnomalyEvent.ResponseTriggered)
        {
            
        }
        else if (anomalyEvent == AnomalyEvent.TimeoutTriggered)
        {
            Debug.Log(" - Penaulty triggered from timeout");
        }
        StartCoroutine(TimerTriggerAnomaly());
    }
}
