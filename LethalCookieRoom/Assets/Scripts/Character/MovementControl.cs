using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour {
    private CharacterController cc;
    private float maxWalkSpeed = 3f;
    private float velocityX = 0f;
    private float runSpeed = 10f;
    private float runAcceleration = 0.5f;
    private float strafeSpeed = 3f;
    private float activeMaxSpeed;

    void Start() {
        cc = gameObject.GetComponent<CharacterController>();
        activeMaxSpeed = maxWalkSpeed;
    }

    void Update() {
        // Run
        if (Input.GetButtonDown("Fire3")) {
            activeMaxSpeed = runSpeed;
        } else if (Input.GetButtonUp("Fire3")) {
            activeMaxSpeed = maxWalkSpeed;
        }

        // Straf
        float strafeDir = 0;
        if (Input.GetAxis("Horizontal") < -0.1f) {
            strafeDir = -1;
        } else if (Input.GetAxis("Horizontal") > 0.1f) {
            strafeDir = 1;
        }
        float strafe = strafeDir * strafeSpeed;
        cc.Move((transform.right * strafe) * Time.deltaTime);

        // Walk
        if (Input.GetAxis("Vertical") > 0.1f) {
            velocityX = activeMaxSpeed;
        } else if (Input.GetAxis("Vertical") < -0.1f) {
            velocityX = -activeMaxSpeed;
        } else {
            velocityX = 0;
        }
        cc.Move((transform.forward * velocityX) * Time.deltaTime);
    }
}