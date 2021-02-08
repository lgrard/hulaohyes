using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public class PlayerStateMachine : StateMachine
    {
        public Running running;
        public Attacking attacking;
        public Carrying carrying;
        private PlayerState _currentState;

        ///Create a new state machine object
        /// <param name="pPlayer">Associated player</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated rigidbody</param>
        public PlayerStateMachine(PlayerController pPlayer, ControlScheme pControlScheme, Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, Transform pAttackPoint)
        {
            running = new Running(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator);
            carrying = new Carrying(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator);
            attacking = new Attacking(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator,pAttackPoint);
            CurrentState = running;
        }
    }
}
