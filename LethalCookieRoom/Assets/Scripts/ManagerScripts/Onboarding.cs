using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onboarding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyManager.Keybinds["ExitInteract"])) {
            Debug.Log("onboarding complete");
            this.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
