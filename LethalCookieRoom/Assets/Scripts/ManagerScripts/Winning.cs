using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winning : MonoBehaviour
{
    public Animation winAnimation;
    public Button winButton;
    public SceneChanger sceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        //winAnimation = this.gameObject.GetComponent<Animation>();
        sceneChanger = GameObject.FindObjectOfType<SceneChanger>();
        winButton.onClick.AddListener(goToMainMenu);
        // this is now set inactive in overlayguide to prevent concurrency issues
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void goToMainMenu() {
        sceneChanger.goToScene("MainMenu");
    }

    public void Win() {
        PauseMenu.isPaused = false;
        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        winAnimation.Play();
    }
}
