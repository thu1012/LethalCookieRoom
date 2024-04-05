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
    // button text to keep setting the text
    public TextMeshProUGUI testInputText;

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
    void Update()
    {
        if(SceneChanger.getCurrentScene() == "MainMenu") {
            if(!mainMenuUI.activeSelf) {
                mainMenuUI.SetActive(true);
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

    public void resume() {
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void pause() {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void openOptionsMenu() {
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
        isInOptions = true;
        testInputText.text = PlayerPrefs.GetString("TestInput");
    }

    public void justOptions() {
        optionsUI.SetActive(true);
        isInOptions = true;
        testInputText.text = PlayerPrefs.GetString("TestInput");
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
