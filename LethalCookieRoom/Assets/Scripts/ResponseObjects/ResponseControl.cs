using InfinityCode.UltimateEditorEnhancer.EditorMenus.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseControl : MonoBehaviour {
    private GameObject triggerSource;
    public enum ResponseType { startAnimation, playAudio, triggerHoldResponse, triggerClickResponse }
    public ResponseType response;

    // Set if startAnimation
    public string animationState;
    // Set if playAudio
    public AudioSource ifPlayAudioResponse;
    // Set if either triggerHoldResponse or triggerClickResponse
    public AnomalyStateMachine anomalyStateMachine;
    public AudioClip triggerAudio;
    public AudioClip completeAudio;
    // Set if triggerHoldResponse
    public float secondsToHold;
    protected float holdDownBeginTime = -1;
    // set if triggerClickResponse
    public int timesToClick;
    protected float lastClickTime;
    protected int timesClicked;

    public virtual void active(GameObject triggerSource) {
        this.triggerSource = triggerSource;
        if (response == ResponseType.startAnimation) { startAnimation(); }
        if (response == ResponseType.playAudio) { playAudio(); }
        if (response == ResponseType.triggerHoldResponse) { triggerHoldResponse(); }
        if (response == ResponseType.triggerClickResponse) { triggerClickResponse(); }
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

    protected void startAnimation(string animation) {
        Animator animator = transform.gameObject.GetComponent<Animator>();
        if (animator != null) {
            animator.Play(animation, -1, 0f);
            animator.enabled = true;
        }
    }

    public void playAudio() {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    protected void playAudio(AudioClip audioClip, float volume) {
        if (audioClip != null) {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null) {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    public void onAnomalyStart(float secondsToHold) {
        this.secondsToHold = secondsToHold;
    }

    public void triggerHoldResponse() {
        if (holdDownBeginTime == -1) { holdDownBeginTime = Time.time; }
        if (Time.time - holdDownBeginTime > secondsToHold) {
            anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
        }
    }

    public void onAnomalyStart(int timeToClick) {
        this.timesToClick = timeToClick;
        timesClicked = 0;
    }

    public virtual void triggerClickResponse() {
        playAudio(triggerAudio, 1f);
        if (Time.time - lastClickTime > 1.5) { timesClicked = 0; }
        lastClickTime = Time.time;
        timesClicked++;
        if (timesClicked == timesToClick) {
            playAudio(completeAudio, 0.5f);
            anomalyStateMachine.TriggerEvent(AnomalyStateMachine.AnomalyEvent.ResponseTriggered);
            timesClicked = 0;
        }
    }
}
