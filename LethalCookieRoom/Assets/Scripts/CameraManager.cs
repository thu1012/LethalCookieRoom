using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject screen;
    public RenderTexture cam1;
    public RenderTexture cam2;

    private int active = 0;

    // Start is called before the first frame update
    void Start()
    {
        screen.GetComponent<Renderer>().material.SetTexture("_EmissionMap", cam1);
        active = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && active != 2)
        {
            screen.GetComponent<Renderer>().material.SetTexture("_EmissionMap", cam2);
            active = 2;
        }
        else if (Input.GetKeyDown("space") && active != 1)
        {
            screen.GetComponent<Renderer>().material.SetTexture("_EmissionMap", cam1);
            active = 1;
        }
    }
}
