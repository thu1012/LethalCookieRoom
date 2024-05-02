using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationControl : MonoBehaviour {
    public GameObject observeObject;
    public AudioClip monitorAudio;
    private GameObject monitor;
    private static KeyManager keyManager;

    void Start() {
        if (monitor == null) {
            monitor = GameObject.Find("/Office/monitor");
        }
        if (keyManager == null) {
            GameObject temp = GameObject.Find("/KeyManager");
            if (temp != null) {
                keyManager = temp.GetComponent<KeyManager>();
            }
        }
    }

    void Update() {
        if (observeObject == monitor) {
            observeObject = monitor;
            monitorUpdate();
        }
        updateExit();
    }

    void monitorUpdate() {
        if ((keyManager != null && Input.GetKeyUp(KeyManager.Keybinds["CameraSwitch"])) ||
            (keyManager == null && (Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonDown(0)))) {
            playAudio(monitorAudio);
            monitor.GetComponent<ScreenControl>().nextCam();
        }
    }

    void updateExit() {
        if ((keyManager != null && Input.GetKeyUp(KeyManager.Keybinds["ExitInteract"])) ||
            (keyManager == null && Input.GetKeyUp(KeyCode.Space))
            /*|| Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)*/) {
            NoteControl nc = observeObject.GetComponent<NoteControl>();
            if (nc != null) { nc.deactivate(gameObject); }
            PostitControl pc = observeObject.GetComponent<PostitControl>();
            if (pc != null) { pc.deactivate(gameObject); }
            MonitorControl monitorControl = observeObject.GetComponent<MonitorControl>();
            if (monitorControl != null) { monitorControl.deactivate(gameObject); }
        }
    }

    private void playAudio(AudioClip audioClip) {
        if (audioClip != null) {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null) {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
