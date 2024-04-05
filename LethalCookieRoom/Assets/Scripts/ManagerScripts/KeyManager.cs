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

        // defaults initialize, add anything needed here
        Keybinds.Add("TestInput", (KeyCode)System.Enum.Parse(typeof(KeyCode), "A"));

        // set from playerprefs if exists (IN PROGRESS AAARRRG)
        // working rn but throwing errors, will redo slightly
        foreach (var kvp in Keybinds)
        {
            if(PlayerPrefs.HasKey(kvp.Key)) {
                Debug.Log("ppref key: " + PlayerPrefs.GetString(kvp.Key));
                Keybinds[kvp.Key] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TestInput"));
                Debug.Log("KeyKey: " + kvp.Key + ", Value: " + kvp.Value);
            }
            else {
                Debug.Log("geeeeet dunked on!!!");
            }
            
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

    // listening to next keypress to rebind
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
                    PlayerPrefs.SetString("TestInput", kcode.ToString());
                    Keybinds[controlFunction] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TestInput"));
                    PlayerPrefs.Save();
                    Debug.Log("playerpref key: " + PlayerPrefs.GetString("TestInput"));
                    Debug.Log("keybinds key: " + Keybinds[controlFunction]);

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
