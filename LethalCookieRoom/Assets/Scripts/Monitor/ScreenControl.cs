using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour {
    public List<RenderTexture> cameraTextures;
    private int currTexture;
    private Material screen;

    void Start() {
        currTexture = 0;
        screen = gameObject.GetComponent<Renderer>().material;
        screen.SetTexture("_Base", cameraTextures[currTexture]);
    }

    public void nextCam() {
        currTexture++;
        if (currTexture >= cameraTextures.Count) {
            currTexture = 0;
        }
        screen.SetTexture("_Base", cameraTextures[currTexture]);
    }

    public void prevCam() {
        currTexture--;
        if (currTexture < 0) {
            currTexture = cameraTextures.Count - 1;
        }
        screen.SetTexture("_Base", cameraTextures[currTexture]);
    }
}