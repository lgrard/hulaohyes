using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class TakingDamage : PlayerState
    {
        private const float STUN_TIME = 1;
        private float _stunStamp;

        public TakingDamage(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetTrigger("TakeDamage");
            _stunStamp = STUN_TIME;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (_stunStamp >= 0) _stunStamp -= Time.deltaTime;
            else _stateMachine.CurrentState = _stateMachine.Running;
        }
    }
}
