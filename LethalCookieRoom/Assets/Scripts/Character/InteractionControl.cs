using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class InteractionControl : MonoBehaviour {
    private float range = 1.5f;
    private Camera cam;

    public List<GameObject> interactables;
    private Dictionary<GameObject, bool> isEmitting;

    private ResponseControl lastInteractedRC;
    private static KeyManager keyManager;

    void Start() {
        cam = Camera.main;
        isEmitting = new Dictionary<GameObject, bool>();
        resetIsEmitting();
        if (keyManager == null) {
            GameObject temp = GameObject.Find("/KeyManager");
            if (temp != null) {
                keyManager = temp.GetComponent<KeyManager>();
            }
        }
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

        resetIsEmitting();
        if (hit.transform != null && interactables.Contains(hit.transform.gameObject) &&
            Vector3.Distance(hit.transform.position, cam.transform.position) < range) {
            EmissionControl ec = hit.transform.gameObject.GetComponent<EmissionControl>();
            if (ec != null) { isEmitting[hit.transform.gameObject] = true; }
            NoteControl nc = hit.transform.gameObject.GetComponent<NoteControl>();
            if (nc != null) { isEmitting[hit.transform.gameObject] = true; }
            PostitControl pc = hit.transform.gameObject.GetComponent<PostitControl>();
            if (pc != null) { isEmitting[hit.transform.gameObject] = true; }
            MonitorControl mc = hit.transform.gameObject.GetComponent<MonitorControl>();
            if (mc != null) { isEmitting[hit.transform.gameObject] = true; }
        }
        updateEmission();
    }

    void resetIsEmitting() {
        foreach (GameObject interactable in interactables) {
            if (isEmitting != null) {
                isEmitting[interactable] = false;
            }
        }
    }

    void updateEmission() {
        foreach (GameObject interactable in interactables) {
            EmissionControl ec = interactable.GetComponent<EmissionControl>();
            if (ec != null) { ec.isEmitting = isEmitting[interactable]; }
            NoteControl nc = interactable.GetComponent<NoteControl>();
            if (nc != null) { nc.isEmitting = isEmitting[interactable]; }
            PostitControl pc = interactable.GetComponent<PostitControl>();
            if (pc != null) { pc.isEmitting = isEmitting[interactable]; }
            MonitorControl mc = interactable.GetComponent<MonitorControl>();
            if (mc != null) { mc.isEmitting = isEmitting[interactable]; }
        }
    }

    void interact(Ray ray) {
        if ((keyManager != null && Input.GetKeyUp(KeyManager.Keybinds["Interact"])) ||
            (keyManager == null && Input.GetMouseButtonDown(0))) {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.white, 0.5f);
            RaycastHit hit;
            Physics.Raycast(ray.origin, ray.direction, out hit);
            if (hit.transform != null && interactables.Contains(hit.transform.gameObject) &&
                Vector3.Distance(hit.transform.position, cam.transform.position) < range) {
                ResponseControl rc = hit.transform.gameObject.GetComponent<ResponseControl>();
                if (rc != null) {
                    if (lastInteractedRC != null && rc != lastInteractedRC) { lastInteractedRC.inactive(gameObject); }
                    lastInteractedRC = rc;
                    rc.active(gameObject);
                } else if (lastInteractedRC != null) {
                    lastInteractedRC.inactive(gameObject);
                    lastInteractedRC = null;
                }
                GetComponent<ObservationControl>().observeObject = hit.transform.gameObject;
            } else if (lastInteractedRC != null) {
                lastInteractedRC.inactive(gameObject);
                lastInteractedRC = null;
            }
        } else if (lastInteractedRC != null) {
            lastInteractedRC.inactive(gameObject);
            lastInteractedRC = null;
        }
    }
}
