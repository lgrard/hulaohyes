using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public class Attacking : Wait
    {
        private NavMeshAgent agent;
        private BoxCollider damageZone;

        public Attacking(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base(pStateMachine, pEnemy, pAnimator)
        { }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            stateMachine.CurrentState = stateMachine.Recovering;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetTrigger("Attacks");
        }
        
        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("Attacking", false);
        }
    }
}
