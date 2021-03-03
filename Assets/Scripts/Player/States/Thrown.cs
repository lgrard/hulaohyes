using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.player.states
{
    public class Thrown : PlayerState
    {
        const float MAX_THROW_TIME = 3f;
        private float _throwTime;

        public Thrown(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }


        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Thrown", true);
            _throwTime = MAX_THROW_TIME;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (_throwTime >= 0) _throwTime -= Time.deltaTime;
            else _stateMachine.CurrentState = _stateMachine.Running;
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Thrown", false);
        }
    }
}
