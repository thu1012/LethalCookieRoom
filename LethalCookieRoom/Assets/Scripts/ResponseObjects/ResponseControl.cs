using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseControl : MonoBehaviour {
    private GameObject triggerSource;
    public enum ResponseType { startAnimation, playAudio, toChair, triggerResponse }
    public List<ResponseType> responses;

    // Set if startAnimation
    public string animationState;
    // Set if playAudio
    public AudioSource audioSource;
    // Set if toChair
    public Vector3 sitPosition;
    // Set if triggerResponse
    public int timeToHold;
    public AnomalyStateMachine anomalyStateMachine;
    private float holdDownBeginTime = -1;

    public virtual void active(GameObject triggerSource) {
        this.triggerSource = triggerSource;
        if (responses.Contains(ResponseType.startAnimation)) { startAnimation(); }
        if (responses.Contains(ResponseType.playAudio)) { playAudio(); }
        if (responses.Contains(ResponseType.toChair)) { toChair(); }
        if (responses.Contains(ResponseType.triggerResponse)) { triggerResponse(); }
    }

    public virtual void inactive(GameObject triggerSource) {
        holdDownBeginTime = -1;
    }

    public void startAnimation() {
        Animator animator = transform.gameObject.GetComponent<Animator>();
        if (animator != null) {
            animator.Play(animationState, -1, 0f);
            animator.enabled = true;
        }
    }

    public void playAudio() {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    public void toChair() {
        triggerSource.GetComponent<PlayerControl>().switchControls(PlayerControl.PlayerState.Sit);
        triggerSource.transform.position = sitPosition;
        triggerSource.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        triggerSource.GetComponent<Camera>().transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void triggerResponse() {
        if (holdDownBeginTime == -1) { holdDownBeginTime = Time.time; }
        if (Time.time - holdDownBeginTime > timeToHold) {
            anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        }
    }
}
