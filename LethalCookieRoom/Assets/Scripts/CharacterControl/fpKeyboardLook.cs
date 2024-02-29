using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpKeyboardLook : MonoBehaviour
{
    private float yRotation;
    [SerializeField] private float rotSpeed = 30.0f;

    // Update is called once per frame
    void Update()
    {
        float turnDir = Input.GetAxis("Horizontal") * rotSpeed  *Time.deltaTime;
        transform.eulerAngles += new Vector3(0, turnDir, 0);
    }
}
