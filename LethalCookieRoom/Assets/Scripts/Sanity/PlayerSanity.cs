using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSanity : MonoBehaviour {

    public Volume sanityVolume;

    private Vignette vignette;
    private Coroutine currentCoroutine;

    void Awake() {
        if (sanityVolume == null) {
            sanityVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        }

        sanityVolume.profile.TryGet(out vignette);
    }

    public void updateCameraFaint(float newSanityVal) {
        float targetIntensity = (100-newSanityVal)/100 * 0.5f + 0.25f;
        Debug.Log(newSanityVal + " :: " + targetIntensity);
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(UpdateVignetteIntensity(targetIntensity));
    }

    private IEnumerator UpdateVignetteIntensity(float targetIntensity) {
        float timeElapsed = 0;
        float duration = 5f;
        float startIntensity = vignette.intensity.value;

        while (timeElapsed < duration) {
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}
