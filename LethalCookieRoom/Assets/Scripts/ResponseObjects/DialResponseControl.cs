using UnityEngine;
using System.Collections;

// TODO: this is a temporary fix for rotating the dial, remember to delete or replace this after M3
public class DialResponseControl : ResponseControl {
    public GameObject dial;

    public override void triggerClickResponse() {
        base.triggerClickResponse();
        dial.transform.Rotate(0f, 30f, 0f, Space.Self)
    }
}
