using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayGuide : MonoBehaviour
{
    // anything managed purely in game scene is here
    // anything that persists is in pausemenu

    public GameObject overlayGuide;
    private GameObject InteractGuide;
    private TextMeshProUGUI interactText;
    private GameObject MonitorGuide;
    private TextMeshProUGUI monitorText;
    public GameObject timeDisplay;
    public TextMeshProUGUI timeText;

    public Winning winScreen;
    public Losing loseScreen;
    private bool won = false;
    private bool lost = false;

    public float startTime = 0.0f;
    public float timePassed = 0.0f;

    private bool hideText;

    public PlayerControl player;
    public SanityControl sanityControl;

    // Start is called before the first frame update
    void Start()
    {
        loseScreen = GameObject.FindObjectOfType<Losing>();
        loseScreen.gameObject.SetActive(false);
        winScreen = GameObject.FindObjectOfType<Winning>();
        winScreen.gameObject.SetActive(false);

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

        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        sanityControl = GameObject.FindObjectOfType<SanityControl>();


        won = false;
        lost = false;
        timePassed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!won && !lost) {
            // if game is happening, hide overlay text when paused only
            // show when not paused
            hideText = PauseMenu.isPaused;
        }
        else {
            // if won or lost, hide overlay text
            hideText = true;
        }

        if(hideText) { // hiding the text when needed
            // monitor control is set to false when paused, so must deactivate floating text here
            showMonitorGuide(false);
            player.switchControls(PlayerControl.PlayerState.Pause);
        }
        else { // returning player to previous state from pausedstate
            if(player.storedState == 0) {
                player.switchControls(PlayerControl.PlayerState.Stand);
            }
            if(player.storedState == 1) {
                // and thus reactivate monitor text here
                showMonitorGuide(true);
                player.switchControls(PlayerControl.PlayerState.Sit);
            }
        }

        if(sanityControl.getSanityVal() == 0 && !won) {
            lost = true;
            loseScreen.Lose();
        }

        // update and check time and win condition
        timePassed = timePassed + Time.deltaTime;
        //Debug.Log(timePassed);
        if (timePassed >= 60 * 10) {
            if(!won) {
                timeText.text = "5am";
                winScreen.Win(); 
                won = true;
            }
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
            string keyToPress = PlayerPrefs.GetString("Interact");
            if(keyToPress == "Mouse0") {
                keyToPress = "Left Click";
            }
            else if(keyToPress == "Mouse1") {
                keyToPress = "Right Click";
            }
            interactText.text = keyToPress + " to interact";
        }
        InteractGuide.SetActive(active && !hideText);
    }

    // called in MonitorControl script
    public void showMonitorGuide(bool active) {
        if(active) {
            string keyToSwitch = PlayerPrefs.GetString("CameraSwitch");
            string keyToExit = PlayerPrefs.GetString("ExitInteract");
            if(keyToSwitch == "Mouse0") {
                keyToSwitch = "Left Click";
            }
            else if(keyToSwitch == "Mouse1") {
                keyToSwitch = "Right Click";
            }
            if(keyToExit == "Mouse0") {
                keyToExit = "Left Click";
            }
            else if(keyToExit == "Mouse1") {
                keyToExit = "Right Click";
            }
            monitorText.text = keyToSwitch + " to change camera\n" + keyToExit + " to stand up";
        }
        MonitorGuide.SetActive(active && !hideText);
    }

}