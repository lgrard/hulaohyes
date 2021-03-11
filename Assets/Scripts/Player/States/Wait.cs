using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public class Wait : PlayerState
    {
        protected float MAX_TIMER = 0.4f;

        private float timer;

        public Wait(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            timer = MAX_TIMER;
            base.animator.SetFloat("Speed", 0);
        }

        protected virtual void TimerEnd()
        {
            stateMachine.CurrentState = stateMachine.Running;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (timer >= 0) timer -= Time.deltaTime;
            else TimerEnd();
        }
    }
}
