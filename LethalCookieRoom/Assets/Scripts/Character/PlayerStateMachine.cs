using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState {
        Stand,
        Sit
    }

    class PlayerStateTransitior {
        protected PlayerState playerState;

        public PlayerStateTransitior(PlayerState playerState) {
            this.playerState = playerState;
        }

        public override bool Equals(object obj) {
            return obj != null && obj is PlayerStateTransitior && (obj as PlayerStateTransitior).playerState == this.playerState;
        }

        public void transit(PlayerState newState) {
            playerState = newState;
            if(newState==PlayerState.Sit) {
                
            }
        }

        public override int GetHashCode() {
            return playerState.GetHashCode();
        }
    }

    PlayerState playerState;
    PlayerStateTransitior transitor;

    public PlayerStateMachine() {
        playerState = PlayerState.Stand;
        transitor = new PlayerStateTransitior(playerState);
    }

    public void setState(PlayerState newState) {
        transitor.transit(newState);
    }
}
