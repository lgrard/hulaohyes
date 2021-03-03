using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public class Wait : PlayerState
    {
        const float MOVEMENT_SPEED = 6f;
        const float MAX_TIMER = 0.5f;
        float _timer;

        public Wait(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _timer = MAX_TIMER;
            base._animator.SetFloat("Speed", _rb.velocity.magnitude / MOVEMENT_SPEED);
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (_timer >= 0) _timer -= Time.deltaTime;
            else _stateMachine.CurrentState = _stateMachine.Running;
        }
    }
}
