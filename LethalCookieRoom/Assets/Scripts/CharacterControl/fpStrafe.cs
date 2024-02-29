using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpStrafe : MonoBehaviour
{
    [SerializeField] private float strafeSpeed = 3f;
    private CharacterController cc;
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
        if (cc==null){Debug.Log("Missing CharacterController");}
    }
    // Update is called once per frame
    void Update()
    {
        float strafeDir = 0;
        if(Input.GetAxis("Horizontal") < -0.1f){
            strafeDir = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0.1f){
            strafeDir = 1;
        }
        float strafe = strafeDir * strafeSpeed;
        cc.Move((transform.right * strafe) * Time.deltaTime);
    }
}
