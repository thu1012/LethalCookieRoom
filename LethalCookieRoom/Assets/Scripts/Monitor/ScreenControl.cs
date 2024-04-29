using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour {
    public List<Material> cameraMaterials;
    public GameObject cameraGlitchAnomalyObject;

    private int maxMaterials;
    private CameraGlitchAnomaly cameraGlitchAnomaly;
    private int currMaterial;

    void Start() {
        cameraGlitchAnomaly = cameraGlitchAnomalyObject.GetComponent<CameraGlitchAnomaly>();
        maxMaterials = cameraMaterials.Count - 1;
        currMaterial = 0;
        SetMaterial();
    }

    public int getCameraMaterial() {
        return currMaterial;
    }

    public void nextCam() {
        currMaterial++;
        if (currMaterial >= maxMaterials) {
            currMaterial = 0;
        }
        SetMaterial();

        if (cameraGlitchAnomaly.getState() == AnomalyStateMachine.AnomalyState.Active) {
            cameraGlitchAnomaly.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        }
    }

    public void prevCam() {
        currMaterial--;
        if (currMaterial < 0) {
            currMaterial = maxMaterials - 1;
        }
        SetMaterial();

        if (cameraGlitchAnomaly.getState() == AnomalyStateMachine.AnomalyState.Active) {
            cameraGlitchAnomaly.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        }
    }

    private void SetMaterial() {
        Material[] materials = GetComponent<Renderer>().materials;
        materials[1] = cameraMaterials[currMaterial];
        GetComponent<Renderer>().materials = materials;
    }

    public void triggerGlitchAnomaly() {
        currMaterial = maxMaterials;
        SetMaterial();
    }
}
