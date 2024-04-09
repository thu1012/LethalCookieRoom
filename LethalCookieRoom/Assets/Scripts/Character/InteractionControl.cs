using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class InteractionControl : MonoBehaviour {
    private float range = 0.75f;
    private Camera cam;

    public List<GameObject> interactables;

    void Start() {
        cam = GetComponentInChildren<Camera>();
    }

    void Update() {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.ScreenPointToRay(screenCenter);
        detect(ray);
        shoot(ray);
    }

    void detect(Ray ray) {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 0.5f);
        if (Physics.Raycast(ray.origin, ray.direction, out hit) && interactables.Contains(hit.transform.gameObject)) {
            hit.transform.gameObject.GetComponent<EmissionControl>().isEmitting = true;
        } else {
            foreach (GameObject interactable in interactables) {
                interactable.GetComponent<EmissionControl>().isEmitting = false;
            }
        }
    }

    void shoot(Ray ray) {
        if (Input.GetMouseButtonDown(0)) {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.white, 0.5f);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit) && interactables.Contains(hit.transform.gameObject)) {
                hit.transform.gameObject.GetComponent<ResponseControl>().active(gameObject);
            }
        }
    }
}
