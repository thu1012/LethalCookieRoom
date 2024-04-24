using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorControl : ResponseControl {
    public List<Material> cameraMaterials;
    public AudioClip audioClip;
    ScreenControl screenControl;
    [HideInInspector] public GameObject interactEmission;
    [HideInInspector] public bool isEmitting;

    private GameObject overlayGuide;

    void Start() {
        screenControl = GetComponent<ScreenControl>();
        if (screenControl == null ) {
            gameObject.AddComponent<ScreenControl>().cameraMaterials = this.cameraMaterials;
        }
        if (overlayGuide == null) {
            overlayGuide = GameObject.Find("OverlayGuide");
        }
        interactEmission = transform.GetChild(0).gameObject;
        interactEmission.SetActive(false);
    }

    void Update() {
        interactEmission.SetActive(isEmitting);
    }

    public override void active(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Sit);
        triggerSource.GetComponent<ObservationControl>().observeObject = gameObject;
        triggerSource.GetComponent<ObservationControl>().monitorAudio = audioClip;
        overlayGuide.GetComponent<OverlayGuide>().showMonitorGuide(true);
        isEmitting = false;
    }
    public void deactivate(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Stand);
        triggerSource.GetComponent<ObservationControl>().observeObject = null;
        overlayGuide.GetComponent<OverlayGuide>().showMonitorGuide(false);
    }

}
