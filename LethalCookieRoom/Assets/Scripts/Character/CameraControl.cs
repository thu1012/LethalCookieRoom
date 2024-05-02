using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private Camera cam;
    private float xRotation = 0;
    private float rotSpeed = 1f;

    void Start() {
        cam = GetComponentInChildren<Camera>();
    }

    void Update() {
        float mouseY = Input.GetAxis("Mouse X") * rotSpeed;
        float mouseX = Input.GetAxis("Mouse Y") * rotSpeed;

        xRotation -= mouseX;
        // Debug.Log(xRotation);
        xRotation = Mathf.Clamp(xRotation, -35, 60);

        transform.eulerAngles += new Vector3(0, mouseY, 0);
        cam.transform.eulerAngles = new Vector3(xRotation, cam.transform.eulerAngles.y, 0);
    }

    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus) {
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
