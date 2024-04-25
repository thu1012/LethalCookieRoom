using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayGuide : MonoBehaviour
{

    public GameObject overlayGuide;
    private GameObject InteractGuide;
    private TextMeshProUGUI interactText;
    private GameObject MonitorGuide;
    private TextMeshProUGUI monitorText;
    private GameObject timeDisplay;
    private TextMeshProUGUI timeText;

    public static float startTime = 60.0f * 10;
    public static float timePassed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        overlayGuide = GameObject.Find("OverlayGuide");
        InteractGuide = GameObject.Find("InteractGuide");
        interactText = InteractGuide.GetComponentInChildren<TextMeshProUGUI>();
        MonitorGuide = GameObject.Find("MonitorGuide");
        monitorText = MonitorGuide.GetComponentInChildren<TextMeshProUGUI>();
        timeDisplay = GameObject.Find("timeDisplay");
        timeText = timeDisplay.GetComponentInChildren<TextMeshProUGUI>();

        InteractGuide.SetActive(false);
        MonitorGuide.SetActive(false);
        //timeDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed = Time.time;
        //Debug.Log(timePassed);

        if (timePassed >= 60 * 10) {
            timeText.text = "you win";
            // do something here
        }
        else if (timePassed >= 60 * 8) {
            timeText.text = "4am";
        }
        else if (timePassed >= 60 * 6) {
            timeText.text = "3am";
        }
        else if (timePassed >= 60 * 4) {
            timeText.text = "2am";
        }
        else if (timePassed >= 60 * 2) {
            timeText.text = "1am";
        }
        else {
            timeText.text = "12am";
        }
    }

    // activated in interactioncontrol script
    // deactivated in emissioncontrol
    public void showInteractGuide(bool active) {
        if(active) {   
            // set text from keymanager
            interactText.text = PlayerPrefs.GetString("Interact") + " to interact";
        }
        InteractGuide.SetActive(active);
    }

    // called in MonitorControl script
    public void showMonitorGuide(bool active) {
        if(active) {   
            monitorText.text = PlayerPrefs.GetString("CameraSwitch") + " to change camera\n" + PlayerPrefs.GetString("ExitInteract") + " to stand up";
        }
        MonitorGuide.SetActive(active);
    }

}