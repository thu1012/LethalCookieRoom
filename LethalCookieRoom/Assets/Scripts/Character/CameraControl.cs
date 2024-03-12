using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour {
    private Camera cam;
    private float xRotation = 0;
    [SerializeField] private float rotSpeed = 1f;

    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        float mouseY = Input.GetAxis("Mouse X") * rotSpeed;
        float mouseX = Input.GetAxis("Mouse Y") * rotSpeed;

        xRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -15, 35);

        transform.eulerAngles += new Vector3(0, mouseY, 0);
        cam.transform.eulerAngles = new Vector3(xRotation, cam.transform.eulerAngles.y, 0);
    }

    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
