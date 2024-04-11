using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorControl : ResponseControl {
    public List<Material> cameraMaterials;
    ScreenControl screenControl;
    public GameObject interactEmission;
    public bool isEmitting;

    void Start() {
        screenControl = GetComponent<ScreenControl>();
        if (screenControl == null ) {
            gameObject.AddComponent<ScreenControl>().cameraMaterials = this.cameraMaterials;
        }
        interactEmission = transform.GetChild(0).gameObject;
        Debug.Log(interactEmission);
        interactEmission.SetActive(false);
    }

    void Update() {
        interactEmission.SetActive(isEmitting);
    }

    public override void active(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Sit);
        triggerSource.GetComponent<ObservationControl>().observeObject = gameObject;
        isEmitting = false;
    }
    public void deactivate(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Stand);
        triggerSource.GetComponent<ObservationControl>().observeObject = null;
    }

}
