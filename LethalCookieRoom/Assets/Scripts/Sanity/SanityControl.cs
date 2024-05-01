using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SanityControl : MonoBehaviour {
    private float sanityVal;
    private int sanityLevel;
    private RoomSanity roomSanity;
    private PlayerSanity playerSanity;

    [Serializable]
    public struct ObjectSanityLevel {
        public GameObject changingObject;
        public int level;
    }

    public ObjectSanityLevel[] changingObjects;
    public List<GameObject> clipboards;
    public Volume sanityVolume;

    void Start() {
        sanityVal = 100f;
        sanityLevel = 0;

        roomSanity = GetComponent<RoomSanity>();
        if(roomSanity == null ) {
            gameObject.AddComponent<RoomSanity>().changingObjects = changingObjects;
            roomSanity = GetComponent<RoomSanity>();
            roomSanity.updateRoom(sanityLevel);
        }
        playerSanity = GetComponent<PlayerSanity>();
        if( playerSanity == null ) {
            gameObject.AddComponent<PlayerSanity>().sanityVolume = this.sanityVolume;
            playerSanity = GetComponent<PlayerSanity>();
        }
        updateBySanity(sanityLevel);
    }

    void Update() {
        // decreaseSanity(Time.deltaTime);
        // Debug.Log(sanityVal);
    }

    public void decreaseSanity(float delta) {
        sanityVal -= delta;
        sanityVal = Math.Max(sanityVal, 0);
        playerSanity.updateCameraFaint(sanityVal);

        int newSanityLevel = getSanityLevel();
        if (newSanityLevel != sanityLevel) {
            updateBySanity(newSanityLevel);
        }
        sanityLevel = newSanityLevel;
        if(sanityVal == 0) {
            PauseMenu.activatePlayerDieUI();
            PauseMenu.isPaused = false;
        }
    }

    void updateBySanity(int newSanityLevel) {
        roomSanity.updateRoom(newSanityLevel);
        updateClipBoards(newSanityLevel);
        Debug.Log("new sanity level::" + newSanityLevel);
    }

    void updateClipBoards(int newSanityLevel) {
        foreach (GameObject clipboard in clipboards) {
            if (clipboard.GetComponent<NoteControl>() != null) {
                clipboard.GetComponent<NoteControl>().updateBoard(newSanityLevel);
            }
            else if (clipboard.GetComponent<PostitControl>() != null) {
                clipboard.GetComponent<PostitControl>().updateBoard(newSanityLevel);
            }
        }
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
