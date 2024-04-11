using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SanityControl;

public class RoomSanity : MonoBehaviour {
    public ObjectSanityLevel[] changingObjects;
    public Camera cam;
    private List<GameObject> toAppear;

    void Start() {
        toAppear = new List<GameObject>();
        if (cam == null ) { cam = Camera.main; }
    }

    void Update() {
        if (toAppear.Count > 0) {
            ShowObjectsOutsideOfView();
        }
    }

    void ShowObjectsOutsideOfView() {
        Vector3 cameraPosition = cam.transform.position;
        foreach (GameObject obj in toAppear) {
            Vector3 directionToObject = obj.transform.position - cameraPosition;
            float angle = Vector3.Angle(cam.transform.forward, directionToObject);

            if (angle > cam.fieldOfView) {
                obj.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void updateRoom(int insanityLevel) {
        foreach (ObjectSanityLevel obj in  changingObjects) {
            if (obj.level == insanityLevel) {
                obj.changingObject.SetActive(true);
                obj.changingObject.GetComponent<Renderer>().enabled = false;
                toAppear.Add(obj.changingObject);
            } else if (obj.level > insanityLevel) { 
                obj.changingObject.SetActive(false);
            }
        }
    }

}
