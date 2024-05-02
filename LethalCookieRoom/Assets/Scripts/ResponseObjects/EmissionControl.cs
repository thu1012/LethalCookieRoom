using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControl : MonoBehaviour {
    private float intensity;
    private Material material;
    [HideInInspector] public bool isEmitting;

    private GameObject overlayGuide;

    void Start() {
        intensity = 2f;
        setEmission();
        isEmitting = false;
        if (overlayGuide == null) {
            overlayGuide = GameObject.Find("OverlayGuide");
        }
    }

    protected virtual void setEmission() {
        material = GetComponentInChildren<Renderer>().materials[1];
    }

    void Update() {
        updateEmission();
    }

    protected virtual void updateEmission() {
        material.SetFloat("_Emission", isEmitting ? intensity : 0f);
        overlayGuide.GetComponent<OverlayGuide>().showInteractGuide(isEmitting);
    }
}
