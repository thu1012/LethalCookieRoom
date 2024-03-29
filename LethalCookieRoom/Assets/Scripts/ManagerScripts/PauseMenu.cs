using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseUI;
    public GameObject optionsUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                resume();
            }
            else {
                pause();
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
    }

    public void exitOptionsMenu() {
        optionsUI.SetActive(false);
        pauseUI.SetActive(true);
    }
}
