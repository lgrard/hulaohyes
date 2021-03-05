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

        public Attacking(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent, BoxCollider pDamageZone)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 3f;
            _agent = pAgent;
            _damageZone = pDamageZone;
        }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            _stateMachine.CurrentState = _stateMachine.Recovering;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _damageZone.enabled = true;
            _agent.speed = 10;
            _agent.angularSpeed = 5;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            _agent.destination = (base._enemy.currentTarget.position);
        }

        public override void OnExit()
        {
            base.OnExit();
            _damageZone.enabled = false;
            _agent.isStopped= true;
            _agent.velocity = _agent.velocity/3;
        }
    }
}
