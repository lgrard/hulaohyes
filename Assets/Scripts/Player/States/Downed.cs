using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Downed : PlayerState
    {
        private const float REVIVAL_TIME = 5;
        private float revivalStamp;
        private bool gettingRevived = false;

        public Downed(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Downed", true);
            revivalStamp = REVIVAL_TIME;
        }
        public override void LoopLogic()
        {
            base.LoopLogic();
            if (revivalStamp > 0 && gettingRevived) revivalStamp -= Time.deltaTime;
            else _stateMachine.CurrentState = _stateMachine.running;
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Downed", false);
        }
    }
}