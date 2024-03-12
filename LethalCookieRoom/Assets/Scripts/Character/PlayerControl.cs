using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateMachine;

public class PlayerControl : MonoBehaviour {
    private MovementControl movementControl;
    private MovementCamera movementCamera;
    private MonitorControl monitorControl;

    // Start is called before the first frame update
    void Start() {
        movementControl = GetComponent<MovementControl>();
        if (movementControl == null) { Debug.Log("Missing Movement Control"); }
        movementCamera = GetComponent<MovementCamera>();
        if (movementCamera == null) { Debug.Log("Missing Movement Camera"); }
        monitorControl = GetComponent<MonitorControl>();
        if (monitorControl == null) { Debug.Log("Missing Monitor Control"); }
    }

    public void switchControls(PlayerState state) {
        switch (state) {
            case PlayerState.Stand:
                movementControl.enabled = true;
                movementCamera.enabled = true;
                monitorControl.enabled = false;
                break;
            case PlayerState.Sit:
                movementControl.enabled = false;
                movementCamera.enabled = false;
                monitorControl.enabled = true;
                break;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
