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
        public Carried carried;
        public TakingDamage takingDamage;
        public Downed downed;

        ///Create a new state machine object
        /// <param name="pPlayer">Associated player</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated rigidbody</param>
        /// <param name="pParticles">Associated player particle systems</param>
        public PlayerStateMachine(PlayerController pPlayer, ControlScheme pControlScheme, Transform pCameraContainer,
            Rigidbody pRb, Animator pAnimator, Transform pAttackPoint, List<ParticleSystem> pParticles)
        {
            running = new Running(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            carrying = new Carrying(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            carried = new Carried(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            takingDamage = new TakingDamage(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            downed = new Downed(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            attacking = new Attacking(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles, pAttackPoint);
            CurrentState = running;
        }
    }
}
