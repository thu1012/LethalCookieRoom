using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class objectTrigger : MonoBehaviour
{
    public enum Effect {nothing, destroyObject, toggleAnimation, startTimeline, triggerParticles, playAudio, teleportPlayer, killPlayer};
    [SerializeField] private Effect effect = Effect.nothing;

    public GameObject targetObject;

    void Start()
    {
        if (targetObject == null){
            Debug.Log("Warning: trigger has no target specified");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate(GameObject player)
    {
        switch (effect)
        {
            case Effect.destroyObject:
                destroryObject();
                break;
            case Effect.toggleAnimation:
                toggleAnimation();
                break;
            case Effect.startTimeline:
                startTimeline();
                break;
            case Effect.playAudio:
                playAudio();
                break;
            case Effect.triggerParticles:
                triggerParticles();
                break;
            case Effect.teleportPlayer:
                teleportPlayer(player);
                break;
            case Effect.killPlayer:
                killPlayer(player);
                break;
        }
    }

    public void toggleAnimation()
    {
        Animator anim = targetObject.transform.gameObject.GetComponent<Animator>();
        if (anim != null) anim.enabled = !anim.enabled;
    }

    public void destroryObject()
    {
        Destroy(targetObject);
    }

    public void startTimeline()
    {
        PlayableDirector pd = targetObject.transform.gameObject.GetComponent<PlayableDirector>();
        if (pd != null && pd.state != PlayState.Playing) pd.Play();
    }

    public void playAudio()
    {
        AudioSource audi = targetObject.transform.gameObject.GetComponent<AudioSource>();
        if (audi != null &&!audi.isPlaying) audi.Play();
    }

    public void triggerParticles()
    {
        ParticleSystem ps = targetObject.transform.gameObject.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    public void teleportPlayer(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = targetObject.transform.position;
            cc.enabled = true;
        }
    }

    public void killPlayer(GameObject player)
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}

