using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.walker
{
    public class Charging : Attacking
    {
        private const float CHARGE_SPEED = 10;

        private NavMeshAgent _agent;
        private BoxCollider _damageZone;

        public Charging(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent, BoxCollider pDamageZone)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 3f;
            _agent = pAgent;
            _damageZone = pDamageZone;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _damageZone.enabled = true;
            _agent.speed = CHARGE_SPEED;
            _agent.angularSpeed = 5;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            _animator.SetFloat("Speed", _agent.velocity.magnitude/ CHARGE_SPEED);
            _agent.destination = (base._enemy.currentTarget.position);
        }

        public override void OnExit()
        {
            base.OnExit();
            _damageZone.enabled = false;
            _agent.isStopped = true;
            _agent.velocity = _agent.velocity / 3;
        }
    }
}
