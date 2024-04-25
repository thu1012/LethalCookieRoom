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
    private GameObject timeDisplay;
    private TextMeshProUGUI timeText;

    public static float startTime = 60.0f * 10;
    public static float timePassed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        overlayGuide = GameObject.Find("OverlayGuide");
        InteractGuide = GameObject.Find("InteractGuide");
        MonitorGuide = GameObject.Find("MonitorGuide");
        timeDisplay = GameObject.Find("timeDisplay");
        timeText = timeDisplay.GetComponentInChildren<TextMeshProUGUI>();

        InteractGuide.SetActive(false);
        MonitorGuide.SetActive(false);
        timeDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed = startTime - Time.time;

    }

    // activated in interactioncontrol script
    // deactivated in emissioncontrol
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

}