using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shapeRotator : MonoBehaviour
{

    // dont commit this this is just for demo and test purposes

    public Vector3 rotateAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAmount * Time.deltaTime);
    }
}
