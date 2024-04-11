using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationControl : MonoBehaviour {
    public GameObject observeObject;
    private GameObject monitor;
    private static KeyManager keyManager;

    void Start() {
        if (monitor == null) {
            monitor = GameObject.Find("/Office/monitor");
        }
        if (keyManager == null) {
            GameObject temp = GameObject.Find("/KeyManager");
            if (temp != null) {
                keyManager = temp.GetComponent<KeyManager>();
            }
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
        if ((keyManager != null && Input.GetKeyUp(KeyManager.Keybinds["CameraSwitch"])) ||
            (keyManager == null && (Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonDown(0)))) {
            monitor.GetComponent<ScreenControl>().nextCam();
        }
    }

    void updateExit() {
        if ((keyManager != null && Input.GetKeyUp(KeyManager.Keybinds["ExitInteract"])) ||
            (keyManager == null && Input.GetKeyUp(KeyCode.Space))
            /*|| Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)*/) {
            ProtocolCBControl cbControl = observeObject.GetComponent<ProtocolCBControl>();
            if (cbControl != null) { cbControl.deactivate(gameObject); }
            MonitorControl monitorControl = observeObject.GetComponent<MonitorControl>();
            if (monitorControl != null) { monitorControl.deactivate(gameObject); }
        }
    }
}
