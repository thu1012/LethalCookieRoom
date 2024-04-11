using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu Instance {get; private set;}

    public static bool isPaused = false;
    public static bool isInOptions = false;

    public GameObject pauseUI;
    public GameObject optionsUI;
    public GameObject mainMenuUI;
    public static GameObject deathUI;
    // button text to keep setting the text
    public TextMeshProUGUI interactButtonText;
    public TextMeshProUGUI cameraSwitchButtonText;
    public TextMeshProUGUI exitInteractButtonText;

    // rebindUI mostly managed by KeyManager
    public static GameObject rebindUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        rebindUI = GameObject.Find("RebindOverlay");
        deathUI = GameObject.Find("DeathScreen");
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        rebindUI.SetActive(false);
        deathUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneChanger.getCurrentScene() == "MainMenu") {
            if(!mainMenuUI.activeSelf) {
                mainMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            if(deathUI.activeSelf) {
                deathUI.SetActive(false);
            }
            //Debug.Log("in main menu!");
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(rebindUI.activeSelf) {
                    rebindUI.SetActive(false);
                }
                else if(isInOptions) {
                    closeJustOptions();
                }
                else {
                    justOptions();                    
                }
            }  
        }
        else if(deathUI.activeSelf /*player is dead*/) {
            // disable player scripts
            //Debug.Log("dead");
        }
        else {
            if(mainMenuUI.activeSelf) {
                mainMenuUI.SetActive(false);
            }
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(rebindUI.activeSelf) {
                    rebindUI.SetActive(false);
                }
                else if(isPaused) {
                    resume();
                }
                else {
                    pause();
                }
            }   
        }
    }

    public static void activatePlayerDieUI() {
        deathUI.SetActive(true);
    }

    public void resume() {
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void pause() {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void openOptionsMenu() {
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
        isInOptions = true;
        interactButtonText.text = PlayerPrefs.GetString("Interact");
        exitInteractButtonText.text = PlayerPrefs.GetString("ExitInteract");
        cameraSwitchButtonText.text = PlayerPrefs.GetString("CameraSwitch");
    }

    public void justOptions() {
        optionsUI.SetActive(true);
        isInOptions = true;
        interactButtonText.text = PlayerPrefs.GetString("Interact");
        exitInteractButtonText.text = PlayerPrefs.GetString("ExitInteract");
        cameraSwitchButtonText.text = PlayerPrefs.GetString("CameraSwitch");
    }

    public void closeJustOptions() {
        optionsUI.SetActive(false);
        isInOptions = false;
    }

    public void exitOptionsMenu() {
        isInOptions = false;
        // need different behavior in main menu
        if(SceneChanger.getCurrentScene() == "MainMenu") {
            optionsUI.SetActive(false);
        }
        else {
            optionsUI.SetActive(false);
            pauseUI.SetActive(true);
        }
    }

    public static void enterRebindUI() {
        rebindUI.SetActive(true);
    }

    public static void exitRebindUI() {
        rebindUI.SetActive(false);
    }
}
