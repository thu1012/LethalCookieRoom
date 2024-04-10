using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtocolCBControl : ResponseControl {
    List<Material> materials = new List<Material>();
    GameObject updateEmission;
    GameObject interactEmission;
    Vector3 originalPosition;
    public bool isEmitting;

    void Start() {
        originalPosition = transform.position;
        interactEmission = transform.GetChild(0).gameObject;
        interactEmission.SetActive(false);
        updateEmission = transform.GetChild(1).gameObject;
        updateEmission.SetActive(false);
    }

    void Update() {
        interactEmission.SetActive(isEmitting);
    }

    public void updateBoard(int level) {
        GetComponent<Renderer>().material = materials[level];
        updateEmission.SetActive(true);
    }

    public override void active(GameObject triggerSource) {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Sit);
        StartCoroutine("moveToPlayer", triggerSource.transform.position);
    }

    public void deactivate() {
        StopCoroutine("moveToPlayer");
        updateEmission.SetActive(false);
        gameObject.transform.position = originalPosition;
    }

    IEnumerator moveToPlayer(Vector3 destination) {
        if (Vector3.Distance(destination, gameObject.transform.position) < 0.5) {
            yield return null;
        }

        yield return null;
    }
}
