using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyManager : MonoBehaviour
{

    public static KeyManager Instance {get; private set;}
    public static Dictionary<string, KeyCode> Keybinds = new Dictionary<string, KeyCode>();

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

    // Start is called before the first frame update
    void Start()
    {
        // TODO: if keybind doesnt exist in playerprefs yet

        // defaults initialize
        Keybinds.Add("TestInput", (KeyCode)System.Enum.Parse(typeof(KeyCode), "A"));
        

        // set from playerprefs (IN PROGRESS AAARRRG)
        foreach (var kvp in Keybinds)
        {
            if(PlayerPrefs.HasKey(kvp.Key)) {
                Debug.Log("key: " + PlayerPrefs.GetString(kvp.Key));
            }
            else {
                Debug.Log("geeeeet dunked on!!!");
            }
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        }
    }

    // needs to listen until key is pressed
    // tmp is to display change to user by changing the button label
    public static void changeKeybind(string controlFunction) {

        // calling rebindUI
        PauseMenu.enterRebindUI();

        // using coroutines to not crash the game
        Instance.StartCoroutine(waitForKey(controlFunction));

    }

    private static IEnumerator waitForKey(string controlFunction) {
        bool waitingForInput = true;

        while(waitingForInput) {
            // if escape, cancel
            if(Input.GetKey(KeyCode.Escape)) {
                waitingForInput = false;
                // exiting rebindUI due to user cancel
                PauseMenu.exitRebindUI();
                break;
            }
            // wait for a key to be pressed
            foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode)) {
                    Debug.Log("KeyCode down: " + kcode);

                    // update keybind dictionary and store in playerprefs
                    Keybinds[controlFunction] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TestInput", kcode.ToString()));

                    // changing text on button when key is rebound
                    // button must be named "(controlname)RebindButton"
                    // this is not great
                    // very inefficient overall but only being called in very specific context so its fine???
                    GameObject test = GameObject.Find((controlFunction + "RebindButton"));
                    if (test != null)
                    {
                        // Get the TextMeshPro component attached to the button
                        TextMeshProUGUI buttonText = test.GetComponentInChildren<TextMeshProUGUI>();
                        buttonText.text = kcode.ToString();

                    }
                    else
                    {
                        Debug.LogError("Button GameObject not found.");
                    }

                    // exiting rebindUI due to successful rebind
                    PauseMenu.exitRebindUI();

                    // remove this!  for debugging!!
                    PlayerPrefs.Save();

                    waitingForInput = false;
                }
            }
            // yield time allows game to continue running and then continue listening (not crash)
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
