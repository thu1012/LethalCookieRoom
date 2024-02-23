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
    Success,
    Fail
}

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

    public AnomalyStateMachine(AnomalyState initState = AnomalyState.Idle)
    {
        currentState = initState;
        transitions = new Dictionary<StateTransitions, AnomalyState>();
        transitions.Add(new StateTransitions(AnomalyState.Idle, AnomalyEvent.Success), AnomalyState.Queued);
        transitions.Add(new StateTransitions(AnomalyState.Queued, AnomalyEvent.Success), AnomalyState.Active);
        transitions.Add(new StateTransitions(AnomalyState.Active, AnomalyEvent.Success), AnomalyState.Queued);
        transitions.Add(new StateTransitions(AnomalyState.Active, AnomalyEvent.Fail), AnomalyState.Queued);
    }

    public AnomalyState getState()
    {
        return currentState;
    }

    public AnomalyState triggerTransition(AnomalyEvent event)
    {
        return currentState;
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

