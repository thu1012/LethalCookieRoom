using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityControl : MonoBehaviour {
    private float sanityVal;
    private int sanityLevel;
    private RoomSanity roomSanity;

    [Serializable]
    public struct ObjectSanityLevel {
        public GameObject changingObject;
        public int level;
    }

    public ObjectSanityLevel[] changingObjects;

    void Start() {
        sanityVal = 100f;
        sanityLevel = 0;

        roomSanity = GetComponent<RoomSanity>();
        if(roomSanity == null ) {
            gameObject.AddComponent<RoomSanity>().changingObjects = changingObjects;
            roomSanity = GetComponent<RoomSanity>();
            roomSanity.updateRoom(sanityLevel);
        }
    }

    public void decreaseSanity(float delta) {
        sanityVal -= delta;
        int newsanityLevel = getSanityLevel();
        if (newsanityLevel != sanityLevel) {
            roomSanity.updateRoom(newsanityLevel);
        }
        sanityLevel = newsanityLevel;
    }

    int getSanityLevel() {
        if (sanityVal >= 85) {
            return 0;
        } else if (sanityVal >= 65) {
            return 1;
        } else if (sanityVal >= 45) {
            return 2;
        } else if (sanityVal >= 25) {
            return 3;
        } else {
            return 4;
        }
    }
}
