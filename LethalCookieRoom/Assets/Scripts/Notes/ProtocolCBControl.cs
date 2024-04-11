using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtocolCBControl : ResponseControl {
    GameObject updateEmission;
    GameObject interactEmission;
    Vector3 originalPosition;
    Quaternion originalRotation;
    public bool isEmitting;
    public List<Material> materials = new List<Material>();

    void Start() {
        interactEmission = transform.GetChild(0).gameObject;
        interactEmission.SetActive(false);
        updateEmission = transform.GetChild(1).gameObject;
        updateEmission.SetActive(false);
    }

    void Update() {
        interactEmission.SetActive(isEmitting);
    }

    public void updateBoard(int level) {
        level = Mathf.Min(level, materials.Count-1);
        GetComponent<Renderer>().material = materials[level];
        updateEmission.SetActive(true);
    }

    public override void active(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Sit);
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        Camera cam = Camera.main;
        transform.position = cam.transform.position + cam.transform.forward;
        transform.rotation = cam.transform.rotation * Quaternion.Euler(90, 0, 180);
        isEmitting = false;
    }

    public void deactivate(GameObject triggerSource) {
        updateEmission.SetActive(false);
        gameObject.transform.position = originalPosition;
        gameObject.transform.rotation = originalRotation;
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Stand);
    }
}
