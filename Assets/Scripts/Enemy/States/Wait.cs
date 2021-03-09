using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public abstract class Wait : EnemyState
    {
        protected float MAX_TIMER = 0.4f;
        private float timer;

        public Wait(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator) : base(pStateMachine, pEnemy, pAnimator) { }

        protected virtual void TimerEnd() { }

        public override void OnEnter()
        {
            base.OnEnter();
            timer = MAX_TIMER;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (timer >= 0) timer -= Time.deltaTime;
            else TimerEnd();
        }
    }
}
