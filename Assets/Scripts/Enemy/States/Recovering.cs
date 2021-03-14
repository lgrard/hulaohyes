using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public class Recovering : Wait
    {
        //private const float ATTACK_RECOVERING_DURATION = 3f;                

        public Recovering(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
        : base(pStateMachine, pEnemy, pAnimator)
        { MAX_TIMER = enemy.ATTACK_RECOVERING_DURATION; }                               //const to change

        protected override void TimerEnd()
        {
            base.TimerEnd();
            stateMachine.CurrentState = stateMachine.Idle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetBool("isRecovering", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("isRecovering", false);
        }
    }
}
