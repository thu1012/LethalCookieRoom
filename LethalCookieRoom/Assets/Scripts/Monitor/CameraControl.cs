using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject screen;
    public List<RenderTexture> cams;
    private int currCam;

    // Start is called before the first frame update
    void Start() {
        currCam = 0;
        screen.GetComponent<Renderer>().material.SetTexture("_EmissionMap", cams[currCam]);
    }

    public void nextCam() {
        currCam++;
        if(currCam >= cams.Count) {
            currCam = 0;
        }
        screen.GetComponent<Renderer>().material.SetTexture("_EmissionMap", cams[currCam]);
    }
}