using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpRun : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float runAcceleration = 0.5f;
    private fpWalk fpw;
    // Start is called before the first frame update
    void Start()
    {
        fpw = gameObject.GetComponent<fpWalk>();
        if (fpw==null){
            fpw = gameObject.AddComponent<fpWalk>();
            Debug.Log("Missing WalkScript");
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3")) {
           fpw.activeMaxSpeed = runSpeed;
           fpw.activeAcceleration = runAcceleration;
        }
        else if (Input.GetButtonUp("Fire3")) {
            fpw.resetSpeed();
        }
    }
}
