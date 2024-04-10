using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class InteractionControl : MonoBehaviour {
    private float range = 10f;
    private Camera cam;

    public List<GameObject> interactables;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.ScreenPointToRay(screenCenter);
        detect(ray);
        interact(ray);
    }

    void detect(Ray ray) {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 0.5f);
        Physics.Raycast(ray.origin, ray.direction, out hit);
        if (hit.transform != null && interactables.Contains(hit.transform.gameObject) &&
            Vector3.Distance(hit.transform.position, cam.transform.position) < range) {
            EmissionControl ec = hit.transform.gameObject.GetComponent<EmissionControl>();
            if (ec != null) { ec.isEmitting = true; }
            ProtocolCBControl pc = hit.transform.gameObject.GetComponent<ProtocolCBControl>();
            if (pc != null) { pc.isEmitting = true; }
        }
        foreach (GameObject interactable in interactables) {
            if (hit.transform !=null && interactable.Equals(hit.transform.gameObject)) { continue; }
            EmissionControl ec = interactable.GetComponent<EmissionControl>();
            if (ec != null) { ec.isEmitting = false; }
            ProtocolCBControl pc = interactable.GetComponent<ProtocolCBControl>();
            if (pc != null) { pc.isEmitting = false; }
        }
    }

    void interact(Ray ray) {
        if (Input.GetMouseButtonDown(0)) {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.white, 0.5f);
            RaycastHit hit;
            Physics.Raycast(ray.origin, ray.direction, out hit);
            if (hit.transform != null && interactables.Contains(hit.transform.gameObject) &&
                Vector3.Distance(hit.transform.position, cam.transform.position) < range) {
                ResponseControl rc = hit.transform.gameObject.GetComponent<ResponseControl>();
                if (rc != null) { rc.active(gameObject); }
                GetComponent<ObservationControl>().observeObject = hit.transform.gameObject;
            }
        }
    }
}
