using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Losing : MonoBehaviour
{
    public Button loseButton;
    public SceneChanger sceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        //winAnimation = this.gameObject.GetComponent<Animation>();
        sceneChanger = GameObject.FindObjectOfType<SceneChanger>();
        loseButton.onClick.AddListener(goToMainMenu);
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

    public void Lose() {
        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
