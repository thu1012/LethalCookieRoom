using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour {
    public List<Material> cameraMaterials;
    private HallucinationAnomaly hallucinationAnomaly;
    private int currMaterial;

    void Start() {
        //hallucinationAnomalymaly = GameObject.Find("HallucinationAnomaly").GetComponent<HallucinationAnomaly>();
        currMaterial = 0;
        SetMaterial();
    }

    public int getCameraMaterial()
    {
        return currMaterial;
    }

    public void nextCam() {
        currMaterial++;
        if (currMaterial >= cameraMaterials.Count) {
            currMaterial = 0;
        }
        SetMaterial();
        //if (hallucinationAnomaly.getState() == AnomalyStateMachine.AnomalyState.Active) {
        //    hallucinationAnomaly.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        //}
    }

    public void prevCam() {
        currMaterial--;
        if (currMaterial < 0) {
            currMaterial = cameraMaterials.Count - 1;
        }
        SetMaterial();
        //    if (hallucinationAnomaly.getState() == AnomalyStateMachine.AnomalyState.Active) {
        //        hallucinationAnomaly.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        //    }
    }

    private void SetMaterial() {
        Material[] materials = GetComponent<Renderer>().materials;
        materials[1] = cameraMaterials[currMaterial];
        GetComponent<Renderer>().materials = materials;
    }

    public void triggerGlitchAnomaly() {
        // TODO: fill in
    }

    public void resolveGlitchAnomaly() {
        // TODO: fill in
    }
}
