using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public class PlayerStateMachine : StateMachine
    {
        private Running _running;
        private Carrying _carrying;
        private Carried _carried;
        private Wait _wait;
        private Downed _downed;
        private Thrown _thrown;
        private Wait _dropping;

        ///Create a new state machine object
        /// <param name="pPlayer">Associated player</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated rigidbody</param>
        /// <param name="pParticles">Associated player particle systems</param>
        public PlayerStateMachine(PlayerController pPlayer, ControlScheme pControlScheme, Transform pCameraContainer,
            Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles, Collider pCollider)
        {
            _running = new Running(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _carrying = new Carrying(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _carried = new Carried(this,pPlayer,pControlScheme,pRb,pAnimator,pParticles,pCollider);
            _wait = new Wait(this,pAnimator,pParticles);
            _downed = new Downed(this,pAnimator,pParticles, pPlayer);
            _thrown = new Thrown(this,pAnimator,pParticles);
            CurrentState = _running;
        }

        public Running Running => _running;
        public Carrying Carrying => _carrying;
        public Carried Carried => _carried;
        public Wait Wait => _wait;
        public Downed Downed => _downed;
        public Thrown Thrown => _thrown;
    }
}
