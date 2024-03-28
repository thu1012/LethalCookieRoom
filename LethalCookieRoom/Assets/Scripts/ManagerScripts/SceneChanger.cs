using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public void SampleScene() {
        Debug.Log("test");
        SceneManager.LoadScene("ingameMenu");
    }

    public void startMenu() {
        Debug.Log("back to start");
        SceneManager.LoadScene("MainMenu");
        // exiting back to main menu, so can unpause as no longer in game anyway
        PauseMenu.isPaused = false;
        Time.timeScale = 1f;
    }

}
