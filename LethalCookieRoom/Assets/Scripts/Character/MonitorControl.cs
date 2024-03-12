using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorControl : MonoBehaviour
{
    public GameObject monitor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F)) {
            monitor.GetComponent<CameraControl>().nextCam();
        }
        if(Input.GetKeyUp(KeyCode.Space)) {
            CharacterController cc = this.GetComponent<CharacterController>();
            if (cc != null) {
                Vector3 targetPosition = new Vector3(0.781f, 0, 6.08f);
                Vector3 moveVector = targetPosition - transform.position;
                cc.Move(moveVector);

                this.gameObject.GetComponent<PlayerControl>().switchControls(PlayerStateMachine.PlayerState.Stand);
            }
        }
    }
}
