using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayGuide : MonoBehaviour
{

    public GameObject overlayGuide;
    private GameObject InteractGuide;
    private GameObject MonitorGuide;

    // Start is called before the first frame update
    void Start()
    {
        overlayGuide = GameObject.Find("OverlayGuide");
        InteractGuide = GameObject.Find("InteractGuide");
        MonitorGuide = GameObject.Find("MonitorGuide");

        InteractGuide.SetActive(false);
        MonitorGuide.SetActive(false);
    }

    // called in interactioncontrol script
    public void showInteractGuide(bool active) {
        if(active) {   
            // set text from keymanager
        }
        InteractGuide.SetActive(active);
    }

    // called in MonitorControl script
    public void showMonitorGuide(bool active) {
        if(active) {   
            // set text from keymanager
        }
        MonitorGuide.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {

    }
}