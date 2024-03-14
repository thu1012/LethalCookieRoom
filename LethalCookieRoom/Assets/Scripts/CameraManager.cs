using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject screen;
    public RenderTexture cam1;
    public RenderTexture cam2;
    public RenderTexture cam3;

    private int active = 0;

    // Start is called before the first frame update
    void Start()
    {
        screen.GetComponent<Renderer>().material.SetTexture("_Base", cam1);
        active = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            switch(active) {

                case 1:
                    screen.GetComponent<Renderer>().material.SetTexture("_Base", cam2);
                    active = 2;
                case 2: 
                    screen.GetComponent<Renderer>().material.SetTexture("_Base", cam3);
                    active = 3;
                case 3: 
                    screen.GetComponent<Renderer>().material.SetTexture("_Base", cam1);
                    active = 1;

            }
        }


    }
}