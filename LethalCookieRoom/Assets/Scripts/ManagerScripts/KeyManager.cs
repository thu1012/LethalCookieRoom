using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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
        Keybinds.Add("TestInput", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TestInput", "A")));
        
        foreach (var kvp in Keybinds)
        {
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        }
    }

    // needs to listen until key is pressed
    public static void changeKeybind(string controlFunction) {

        // using coroutines to not crash the game
        Instance.StartCoroutine(waitForKey(controlFunction));

        // TODO: save to player prefs, update button text, give feedback
        // add another screen to tell player to press a key to bind it

    }

    private static IEnumerator waitForKey(string controlFunction) {
        bool waitingForInput = true;

        while(waitingForInput) {
            // if escape, cancel
            if(Input.GetKey(KeyCode.Escape)) {
                waitingForInput = false;
                break;
            }
            // wait for a key to be pressed
            foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(kcode)) {
                    Debug.Log("KeyCode down: " + kcode);
                    Keybinds[controlFunction] = kcode;
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
