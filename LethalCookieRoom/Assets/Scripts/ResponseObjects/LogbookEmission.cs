using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogbookEmission : EmissionControl {
    private GameObject interactEmission;

    protected override void setEmission() {
        interactEmission = transform.GetChild(0).gameObject;
        interactEmission.SetActive(false);
    }

    protected override void updateEmission() {
        interactEmission.SetActive(isEmitting);
    }
}
