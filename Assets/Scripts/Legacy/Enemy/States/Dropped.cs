using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public class Dropped : Wait
    {
        //private const float DROP_RECOVER_DURATION = 1f;                

        public Dropped(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
        : base(pStateMachine, pEnemy, pAnimator)
        { MAX_TIMER = enemy.DROP_RECOVER_DURATION; }                               //const to change

        protected override void TimerEnd()
        {
            base.TimerEnd();
            stateMachine.CurrentState = stateMachine.Idle;
        }
    }
}
