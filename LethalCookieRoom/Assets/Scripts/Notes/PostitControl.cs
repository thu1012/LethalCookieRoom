using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PostitControl : ResponseControl {
    // GameObject updateEmission;
    GameObject interactEmission;
    Vector3 originalPosition;
    Quaternion originalRotation;
    Renderer renderor;
    MeshCollider meshCollider;

    public bool isEmitting;
    public int levelToShow;
    public float distanceWhenZoomed;
    int level = -1;

    void Start() {
        interactEmission = transform.GetChild(0).gameObject;
        interactEmission.SetActive(false);
        // updateEmission = transform.GetChild(1).gameObject;
        // updateEmission.SetActive(false);
        renderor = GetComponent<MeshRenderer>();
        renderor.enabled = false;
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = false;
        if (distanceWhenZoomed == 0) {
            distanceWhenZoomed = 0.31f;
        }
    }

    void Update() {
        if (level >= levelToShow - 1) {
            interactEmission.SetActive(isEmitting);
        }
    }

    public void updateBoard(int level) {
        Debug.Log(gameObject.name + ": updateBoard");
        this.level = level;
        if (level >= levelToShow-1) {
            renderor.enabled = true;
            meshCollider.enabled = true;
            // updateEmission.SetActive(true);
        }
    }

    public override void active(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Sit);
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        Camera cam = Camera.main;
        transform.position = cam.transform.position + cam.transform.forward*distanceWhenZoomed;
        transform.rotation = cam.transform.rotation * Quaternion.Euler(90, 0, 180);

        isEmitting = false;
    }

    public void deactivate(GameObject triggerSource) {
        // updateEmission.SetActive(false);
        gameObject.transform.position = originalPosition;
        gameObject.transform.rotation = originalRotation;
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Stand);
    }
}
