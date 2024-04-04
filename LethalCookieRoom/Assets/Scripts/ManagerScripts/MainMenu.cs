using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject optionsUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(optionsUI.activeSelf == true) {
                mainMenuResume();
            }
            else {
                mainMenuOptions();
            }
        }    
    }

    public void mainMenuResume() {
        optionsUI.SetActive(false);
    }

    public void mainMenuOptions() {
        optionsUI.SetActive(true);
    }

}
