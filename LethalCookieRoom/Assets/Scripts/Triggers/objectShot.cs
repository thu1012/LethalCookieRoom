using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;



public class objectShot : MonoBehaviour
{
    public enum Effect {nothing, destroyObject, toggleAnimation, startTimeline, playAudio, toChair};
    [SerializeField] private Effect effect = Effect.nothing;
    public Camera playerCamera;
    public string animationState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Destroy Effect
    public void destroyObject() {
        Destroy(gameObject);
    }

    //Animation Effect
    public void toggleAnimation(RaycastHit hit, GameObject player) {
        if(!rangeCheck(hit.transform.position, player.transform.position, 7f)) {
            return;
        }
        Animator anim = hit.transform.gameObject.GetComponent<Animator>();
        if (anim != null) {
            anim.Play(animationState, -1, 0f);
            anim.enabled = true;
        }
    }

    // Timeline Effect
    public void startTimeline(RaycastHit hit) {
        PlayableDirector pd = hit.transform.gameObject.GetComponent<PlayableDirector>();
        if (pd != null) {
            if (pd.state != PlayState.Playing) {
                pd.Play();
            }
        }
    }

    //Audio Effect
    public void playAudio(RaycastHit hit) {
        AudioSource audi = hit.transform.gameObject.GetComponent<AudioSource>();
        if (audi != null) {
            if (!audi.isPlaying) {
                audi.Play();
            }
        }
    }

    public void triggerParticles(RaycastHit hit) {
        ParticleSystem ps = hit.transform.gameObject.GetComponent<ParticleSystem>();
        if (ps != null) {
            ps.Play();

        }
    }

    private bool rangeCheck(Vector3 obj1, Vector3 obj2, float range) {
        if (Vector3.Distance(obj1, obj2) > range) {
            return false;
        }
        return true;
    }

    public void toChair(GameObject player) {
        Vector3 chairPos = new Vector3(-0.478f, 0, 6.08f);
        if(!rangeCheck(chairPos, player.transform.position, 3f)) {
            return;
        }
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) {
            player.GetComponent<PlayerControl>().switchControls(PlayerStateMachine.PlayerState.Sit);
            player.transform.position = chairPos;
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            playerCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void Activate(GameObject player, Ray ray, RaycastHit hit) {
        switch (effect) {
            case Effect.destroyObject:
                destroyObject();
                break;
            case Effect.toggleAnimation:
                toggleAnimation(hit, player);
                break;
            case Effect.startTimeline:
                startTimeline(hit);
                break;
            case Effect.playAudio:
                playAudio(hit);
                break;
            case Effect.toChair:
                toChair(player);
                break;
        }
    }
}
