using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public class StartUp : Wait
    {
        private const float ROTATION_AMOUNT = 0.01f;

        public StartUp(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 3f;
        }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            stateMachine.CurrentState = stateMachine.Attacking;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            Vector3 lDirection = enemy.currentTarget.position - enemy.transform.position;
            Quaternion lRotation = Quaternion.LookRotation(lDirection, Vector3.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lRotation, ROTATION_AMOUNT);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetBool("Attacking", true);
        }
    }
}
