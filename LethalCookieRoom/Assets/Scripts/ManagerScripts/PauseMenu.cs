using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    // I humbly apologize for this monstrosity

    public static PauseMenu Instance {get; private set;}

    public static bool isPaused = false;
    public static bool isInOptions = false;

    public GameObject pauseUI;
    public GameObject optionsUI;
    public GameObject mainMenuUI;
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

        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        rebindUI.SetActive(false);

    }

    // Update is called once per frame
    // Update manages pause behavior
    void Update()
    {
        if(SceneChanger.getCurrentScene() == "MainMenu") {
            if(!mainMenuUI.activeSelf) {
                mainMenuUI.SetActive(true);
                pauseUI.SetActive(false);
                optionsUI.SetActive(false);
                // enter main menu, set cursor free
                Cursor.lockState = CursorLockMode.None;
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

        // if not in main menu
        else {

            if(mainMenuUI.activeSelf) {
                // set main menu to false when not there
                mainMenuUI.SetActive(false);
            }

            if(Input.GetKeyDown(KeyCode.Escape) && SceneChanger.getCurrentScene() != "LoreScreen") {

                if(rebindUI.activeSelf) {
                    rebindUI.SetActive(false);
                }

                else if(!isPaused) {
                    pause();
                }

            }   
        }
    }

    public void resume() {
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        // resuming, lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("resuming");
    }

    void pause() {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        // pausing, free cursor
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
