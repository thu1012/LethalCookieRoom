using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AnomalyState
{
    Idle,
    Queued,
    Active
}

enum AnomalyEvent
{
    QueueAnomaly,
    TriggerAnomaly,
    ResponseTriggered,
    TimeoutTriggered
}

delegate void StateAction(AnomalyEvent anomalyEvent);

class AnomalyStateMachine
{
    class StateTransitions
    {
        protected AnomalyState anomalyStates;
        protected AnomalyEvent anomalyEvents;

        public StateTransitions(AnomalyState anomalyStates, AnomalyEvent anomalyEvents)
        {
            this.anomalyStates = anomalyStates;
            this.anomalyEvents = anomalyEvents;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is StateTransitions && (obj as StateTransitions).anomalyStates == this.anomalyStates && (obj as StateTransitions).anomalyEvents == this.anomalyEvents;
        }
    }

    AnomalyState currentState;
    Dictionary<StateTransitions, AnomalyState> transitions;
    Dictionary<AnomalyState, StateAction> entryActions;
    Dictionary<AnomalyState, StateAction> exitActions;

    public AnomalyStateMachine(AnomalyState initState = AnomalyState.Idle)
    {
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

    public AnomalyState getState()
    {
        return currentState;
    }

    public AnomalyState TriggerEvent(AnomalyEvent anomalyEvent)
    {
        StateTransitions transitionKey = new StateTransitions(currentState, anomalyEvent);

        if (transitions.ContainsKey(transitionKey))
        {
            AnomalyState newState = transitions[transitionKey];

            exitActions[currentState](anomalyEvent);
            entryActions[newState](anomalyEvent);

            currentState = newState;
        }
        return currentState;
    }

    private void onIdleEnter(AnomalyEvent anomalyEvent)
    {

    }

    private void onQueuedEnter(AnomalyEvent anomalyEvent)
    {

    }

    private void onActiveEnter(AnomalyEvent anomalyEvent)
    {

    }

    private void onIdleExit(AnomalyEvent anomalyEvent)
    {

    }

    private void onQueuedExit(AnomalyEvent anomalyEvent)
    {

    }

    private void onActiveExit(AnomalyEvent anomalyEvent)
    {

    }
}

public class AnomalySystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

