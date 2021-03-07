using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public class Attacking : Wait
    {
        private NavMeshAgent _agent;
        private BoxCollider _damageZone;

        public Attacking(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base(pStateMachine, pEnemy, pAnimator)
        { }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            _stateMachine.CurrentState = _stateMachine.Recovering;
        }
    }
}
