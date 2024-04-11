using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationControl : MonoBehaviour {
    public GameObject observeObject;
    private GameObject monitor;

    void Start() {
        if (monitor == null) {
            monitor = GameObject.Find("/Office/monitor");
        }
    }

    void Update() {
        if (observeObject == monitor) {
            observeObject = monitor;
            monitorUpdate();
        }
        updateExit();
    }

    void monitorUpdate() {
        if (Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonDown(0)) {
            monitor.GetComponent<ScreenControl>().nextCam();
        }
    }

    void updateExit() {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            ProtocolCBControl cbControl = observeObject.GetComponent<ProtocolCBControl>();
            if (cbControl != null ) { cbControl.deactivate(gameObject); }
            MonitorControl monitorControl = observeObject.GetComponent<MonitorControl>();
            if (monitorControl != null ) { monitorControl.deactivate(gameObject); }
        }
    }
}
