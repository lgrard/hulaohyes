using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public class Wait : PlayerState
    {
        protected float MAX_TIMER = 0.4f;

        private float _timer;

        public Wait(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _timer = MAX_TIMER;
            base._animator.SetFloat("Speed", 0);
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (_timer >= 0) _timer -= Time.deltaTime;
            else _stateMachine.CurrentState = _stateMachine.Running;
        }
    }
}
