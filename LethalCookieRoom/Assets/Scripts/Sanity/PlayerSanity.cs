using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSanity : MonoBehaviour {

    public Volume sanityVolume;

    private Vignette vignette;

    void Awake() {
        if (sanityVolume == null) {
            sanityVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        }

        sanityVolume.profile.TryGet(out vignette);
    }

    public void updateCameraFaint(int sanityLevel) {
        switch (sanityLevel) {
            case 0:
                vignette.intensity.value = 0.25f;
                break;
            case 1:
                vignette.intensity.value = 0.3f;
                break;
            case 2:
                vignette.intensity.value = 0.35f;
                break;
            case 3:
                vignette.intensity.value = 0.45f;
                break;
            case 4:
                vignette.intensity.value = 0.5f;
                break;
        }
    }
}
