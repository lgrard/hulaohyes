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
        private TakingDamage _takingDamage;
        private Downed _downed;
        private Thrown _thrown;

        ///Create a new state machine object
        /// <param name="pPlayer">Associated player</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated rigidbody</param>
        /// <param name="pParticles">Associated player particle systems</param>
        public PlayerStateMachine(PlayerController pPlayer, ControlScheme pControlScheme, Transform pCameraContainer,
            Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
        {
            _running = new Running(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _carrying = new Carrying(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _carried = new Carried(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _takingDamage = new TakingDamage(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _downed = new Downed(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            _thrown = new Thrown(this, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles);
            CurrentState = _running;
        }

        public Running Running => _running;
        public Carrying Carrying => _carrying;
        public Carried Carried => _carried;
        public TakingDamage TakingDamage => _takingDamage;
        public Downed Downed => _downed;
        public Thrown Thrown => _thrown;
    }
}
