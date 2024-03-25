using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationControl : MonoBehaviour {
    private GameObject monitor;

    void Start() {
        if (monitor == null) {
            monitor = GameObject.Find("/Office/monitor");
        }
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.F)) {
            monitor.GetComponent<ScreenControl>().nextCam();
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            CharacterController cc = this.GetComponent<CharacterController>();
            if (cc != null) {
                Vector3 targetPosition = new Vector3(0.781f, 0, 6.08f);
                Vector3 moveVector = targetPosition - transform.position;
                cc.Move(moveVector);

                this.gameObject.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Stand);
            }
        }
    }
}
