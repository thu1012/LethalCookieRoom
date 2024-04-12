using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour {
    public List<Material> cameraMaterials;
    public HallucinationAnomaly hallucinationAnomaly;
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
        //if (hallucinationAnomaly.getAnomalyReady()) {
        //    triggerHallucinationAnomaly();
        //} else if (hallucinationAnomaly.getState() == AnomalyStateMachine.AnomalyState.Active) {
        //    resolveHallucinationAnomaly();
        //}
    }

    public void prevCam() {
        currMaterial--;
        if (currMaterial < 0) {
            currMaterial = cameraMaterials.Count - 1;
        }
        SetMaterial();
        //if (hallucinationAnomaly.getAnomalyReady()) {
        //    triggerHallucinationAnomaly();
        //} else if (hallucinationAnomaly.getState() == AnomalyStateMachine.AnomalyState.Active) {
        //    resolveHallucinationAnomaly();
        //}
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

    private void triggerHallucinationAnomaly() {
        hallucinationAnomaly.TriggerEvent(AnomalyStateMachine.AnomalyEvent.TriggerAnomaly);
        // TODO: spawn in dude
    }

    private void resolveHallucinationAnomaly() {
        hallucinationAnomaly.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        // TODO: despawn dude
    }
}
