using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpWalk : MonoBehaviour
{
    [SerializeField] private float maxWalkSpeed = 3f;
    [SerializeField] private bool useAcceleration = false;
    [SerializeField] private float accelerationRate = 0.1f;
    [SerializeField] private float friction = 0.01f;
    private float velocityX = 0f;
    [HideInInspector] public float activeMaxSpeed;
    [HideInInspector] public float activeAcceleration;
    private CharacterController cc;

    void Start()
    {
        //This makes sure there is a Character Controller component so that the script can run
        cc = gameObject.GetComponent<CharacterController>();
        if (cc==null){Debug.Log("Missing CharacterController");}

        //Because other scripts (like running) can change these speeds, we want to keep the current numbers seperate from
        //the original numbers in case we ever need to restore them.
        activeMaxSpeed = maxWalkSpeed;
        activeAcceleration = accelerationRate;
        
    }
    // Update is called once per frame
    void Update()
    {
        //Walking with acceleration
        if(useAcceleration){
            //Forward
            if(Input.GetAxis("Vertical") > 0.1f){
                velocityX += activeAcceleration;
                //Makes sure we don't go over the max speed
                velocityX = Mathf.Min(velocityX, activeMaxSpeed);
            }
            //Backward
            else if (Input.GetAxis("Vertical") < -0.1f){
                velocityX -=  activeAcceleration;
                velocityX = Mathf.Max(velocityX, -activeMaxSpeed);
            }
            //Deceleration
            else {
                if(velocityX > friction){
                    velocityX -= friction;
                }
                else if (velocityX < -friction){
                    velocityX += friction;
                }
                else {
                    velocityX = 0;
                }
            }
        }
        //Walking without acceleration
        else{
            //Forward
            if(Input.GetAxis("Vertical") > 0.1f){
                velocityX = activeMaxSpeed;
            }
            //Backward
            else if (Input.GetAxis("Vertical") < -0.1f){
                velocityX = -activeMaxSpeed;
            }
            else{
                velocityX = 0;
            }
        }

        //Uses the velocity we've calculated to actually move the character
        cc.Move((transform.forward * velocityX) * Time.deltaTime);
    }
    
public void resetSpeed() {
        activeMaxSpeed = maxWalkSpeed;
        activeAcceleration = accelerationRate;
    
}

        
}