using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public static SceneChanger Instance {get; private set;}
    public static string currentScene;

    void Start() {
        currentScene = SceneManager.GetActiveScene().name;
    }

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


    // generalized scene changer
    public void goToScene(string sceneToGo) {
        Debug.Log("test2");
        currentScene = sceneToGo;
        SceneManager.LoadScene(sceneToGo);
        if(sceneToGo == "Main Scene 0501") {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // old test function, could delete now
    public void SampleScene() {
        Debug.Log("test");
        currentScene = "ingameMenu";
        SceneManager.LoadScene("ingameMenu");
    }

    // dont change this
    public void startMenu() {
        Debug.Log("back to start");
        currentScene = "MainMenu";
        SceneManager.LoadScene("MainMenu");
        // exiting back to main menu, so can unpause as no longer in game anyway
        PauseMenu.isPaused = false;
        Time.timeScale = 1f;
    }

    public static string getCurrentScene() {
        return currentScene;
    }

}
