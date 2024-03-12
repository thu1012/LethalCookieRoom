using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour {
    [SerializeField] private float maxWalkSpeed = 3f;
    private float velocityX = 0f;
    [HideInInspector] public float activeMaxSpeed;
    [HideInInspector] public float activeAcceleration;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float runAcceleration = 0.5f;
    [SerializeField] private float strafeSpeed = 3f;
    private CharacterController cc;

    void Start() {
        //This makes sure there is a Character Controller component so that the script can run
        cc = gameObject.GetComponent<CharacterController>();
        if (cc == null) { Debug.Log("Missing CharacterController"); }

        //Because other scripts (like running) can change these speeds, we want to keep the current numbers seperate from
        //the original numbers in case we ever need to restore them.
        activeMaxSpeed = maxWalkSpeed;
    }

    // Update is called once per frame
    void Update() {
        // Run
        if (Input.GetButtonDown("Fire3")) {
            activeMaxSpeed = runSpeed;
            activeAcceleration = runAcceleration;
        } else if (Input.GetButtonUp("Fire3")) {
            resetSpeed();
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

        //Uses the velocity we've calculated to actually move the character
        cc.Move((transform.forward * velocityX) * Time.deltaTime);
    }

    public void resetSpeed() {
        activeMaxSpeed = maxWalkSpeed;
    }
}