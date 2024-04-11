using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour {
    public List<Material> cameraMaterials;
    private int currMaterial;

    void Start() {
        currMaterial = 0;
        SetMaterial();
    }

    public void nextCam() {
        currMaterial++;
        if (currMaterial >= cameraMaterials.Count) {
            currMaterial = 0;
        }
        SetMaterial();
    }

    public void prevCam() {
        currMaterial--;
        if (currMaterial < 0) {
            currMaterial = cameraMaterials.Count - 1;
        }
        SetMaterial();
    }

    private void SetMaterial() {
        Material[] materials = GetComponent<Renderer>().materials;
        materials[1] = cameraMaterials[currMaterial];
        GetComponent<Renderer>().materials = materials;
    }
}
