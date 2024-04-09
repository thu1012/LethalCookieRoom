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

    public void SampleScene() {
        Debug.Log("test");
        currentScene = "ingameMenu";
        SceneManager.LoadScene("ingameMenu");
    }

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
