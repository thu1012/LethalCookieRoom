using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreScreen : MonoBehaviour
{
    public Button applyNowButton;
    public SceneChanger sceneChanger;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        sceneChanger = GameObject.FindObjectOfType<SceneChanger>();
        applyNowButton.onClick.AddListener(goToGame);
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void goToGame() {
        sceneChanger.goToScene("Main Scene 0430");
    }

    public void playAudio(AudioClip sound) {
        audioSource.clip = sound;
        audioSource.Play();
    }
}
