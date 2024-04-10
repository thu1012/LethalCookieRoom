using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;
using System.Collections;

public class AnomalyStateMachine : MonoBehaviour {
    public enum AnomalyState {
        Idle,
        Queued,
        Active
    }

    public enum AnomalyEvent {
        QueueAnomaly,
        TriggerAnomaly,
        ResponseTriggered,
        TimeoutTriggered
    }

    delegate void StateAction(AnomalyEvent anomalyEvent);

    class StateTransitions {
        protected AnomalyState anomalyState;
        protected AnomalyEvent anomalyEvent;

        public StateTransitions(AnomalyState anomalyState, AnomalyEvent anomalyEvent) {
            this.anomalyState = anomalyState;
            this.anomalyEvent = anomalyEvent;
        }

        public override bool Equals(object obj) {
            return obj != null && obj is StateTransitions && (obj as StateTransitions).anomalyState == this.anomalyState && (obj as StateTransitions).anomalyEvent == this.anomalyEvent;
        }

        public override int GetHashCode() {
            return HashCode.Combine(anomalyState, anomalyEvent);
        }
    }

    public int timeoutTriggerSeconds;
    public int anomalyTriggerSeconds;
    public double anomalyTriggerProbability;
    public int sanityPenalty;

    AnomalyState currentState;
    Dictionary<StateTransitions, AnomalyState> transitions;
    Dictionary<AnomalyState, StateAction> entryActions;
    Dictionary<AnomalyState, StateAction> exitActions;

    protected void initStateMachine(int timeoutTriggerSeconds, int anomalyTriggerSeconds, double anomalyTriggerProbability, AnomalyState initState = AnomalyState.Idle) {
        this.timeoutTriggerSeconds = timeoutTriggerSeconds;
        this.anomalyTriggerSeconds = anomalyTriggerSeconds;
        this.anomalyTriggerProbability = anomalyTriggerProbability;
        currentState = initState;
        transitions = new Dictionary<StateTransitions, AnomalyState>();
        entryActions = new Dictionary<AnomalyState, StateAction>();
        exitActions = new Dictionary<AnomalyState, StateAction>();

        transitions.Add(new StateTransitions(AnomalyState.Idle, AnomalyEvent.QueueAnomaly), AnomalyState.Queued);
        transitions.Add(new StateTransitions(AnomalyState.Queued, AnomalyEvent.TriggerAnomaly), AnomalyState.Active);
        transitions.Add(new StateTransitions(AnomalyState.Queued, AnomalyEvent.ResponseTriggered), AnomalyState.Queued);
        transitions.Add(new StateTransitions(AnomalyState.Active, AnomalyEvent.ResponseTriggered), AnomalyState.Queued);
        transitions.Add(new StateTransitions(AnomalyState.Active, AnomalyEvent.TimeoutTriggered), AnomalyState.Queued);

        entryActions.Add(AnomalyState.Idle, onIdleEnter);
        entryActions.Add(AnomalyState.Queued, onQueuedEnter);
        entryActions.Add(AnomalyState.Active, onActiveEnter);

        exitActions.Add(AnomalyState.Idle, onIdleExit);
        exitActions.Add(AnomalyState.Queued, onQueuedExit);
        exitActions.Add(AnomalyState.Active, onActiveExit);
    }

    public AnomalyState getState() {
        return currentState;
    }

    public AnomalyState TriggerEvent(AnomalyEvent anomalyEvent) {
        StateTransitions transitionKey = new StateTransitions(currentState, anomalyEvent);

        if (transitions.ContainsKey(transitionKey)) {
            AnomalyState newState = transitions[transitionKey];

            exitActions[currentState](anomalyEvent);
            entryActions[newState](anomalyEvent);

            currentState = newState;
        }
        return currentState;
    }

    protected virtual void onIdleEnter(AnomalyEvent anomalyEvent) {
        Debug.LogError($"{this.GetType()} called virtual function onIdleEnter without override");
    }

    protected virtual void onQueuedEnter(AnomalyEvent anomalyEvent) {
        Debug.LogError($"{this.GetType()} called virtual function onQueuedEnter without override");
    }

    protected virtual void onActiveEnter(AnomalyEvent anomalyEvent) {
        Debug.LogError($"{this.GetType()} called virtual function onActiveEnter without override");
    }

    protected virtual void onIdleExit(AnomalyEvent anomalyEvent) {
        Debug.LogError($"{this.GetType()} called virtual function onIdleExit without override");
    }

    protected virtual void onQueuedExit(AnomalyEvent anomalyEvent) {
        Debug.LogError($"{this.GetType()} called virtual function onQueuedExit without override");
    }

    protected virtual void onActiveExit(AnomalyEvent anomalyEvent) {
        Debug.LogError($"{this.GetType()} called virtual function onActiveExit without override");
    }

    protected IEnumerator TimerTriggerAnomaly() {
        //Debug.LogFormat($"Triggering anomaly in {time} seconds");
        yield return new WaitForSecondsRealtime(anomalyTriggerSeconds);

        Random random = new Random();
        if (random.NextDouble() < anomalyTriggerProbability) {
            TriggerEvent(AnomalyEvent.TriggerAnomaly);
        } else {
            StartCoroutine(TimerTriggerAnomaly());
        }
    }

    protected IEnumerator TimerTriggerTimeout() {
        //Debug.LogFormat($"Triggering timeout in {time} seconds");
        yield return new WaitForSecondsRealtime(timeoutTriggerSeconds);

        TriggerEvent(AnomalyEvent.TimeoutTriggered);
    }
}
