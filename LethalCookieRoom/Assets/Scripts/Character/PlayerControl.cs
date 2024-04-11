using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    private MovementControl movementControl;
    private CameraControl cameraControl;
    private ObservationControl observationControl;
    private InteractionControl interactionControl;

    [HideInInspector] public enum PlayerState { Stand, Sit }
    public List<GameObject> interactables;

    void Start() {
        movementControl = GetComponent<MovementControl>();
        if (movementControl == null) {
            gameObject.AddComponent<MovementControl>();
            movementControl = GetComponent<MovementControl>();
        }
        cameraControl = GetComponent<CameraControl>();
        if (cameraControl == null) {
            gameObject.AddComponent<CameraControl>();
            cameraControl = GetComponent<CameraControl>();
        }
        observationControl = GetComponent<ObservationControl>();
        if (observationControl == null) {
            gameObject.AddComponent<ObservationControl>();
            observationControl = GetComponent<ObservationControl>();
        }
        interactionControl = GetComponent<InteractionControl>();
        if (interactionControl == null) {
            gameObject.AddComponent<InteractionControl>().interactables = this.interactables; ;
            interactionControl = GetComponent<InteractionControl>();
        }
        switchControls(PlayerState.Stand);
    }

    public void switchControls(PlayerState state) {
        switch (state) {
            case PlayerState.Stand:
                movementControl.enabled = true;
                cameraControl.enabled = true;
                interactionControl.enabled = true;
                observationControl.enabled = false;
                break;
            case PlayerState.Sit:
                movementControl.enabled = false;
                cameraControl.enabled = false;
                interactionControl.enabled = false;
                observationControl.enabled = true;
                break;

        }
    }
}
