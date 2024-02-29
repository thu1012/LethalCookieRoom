using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider trig){
        objectTrigger ct = trig.transform.gameObject.GetComponent<objectTrigger>();
                if (ct != null){
                    ct.Activate(this.gameObject);
                }

    }
}
