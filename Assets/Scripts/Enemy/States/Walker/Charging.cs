using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.walker
{
    public class Charging : Attacking
    {
        private const float CHARGE_SPEED = 10;

        private NavMeshAgent agent;
        private BoxCollider damageZone;

        public Charging(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent, BoxCollider pDamageZone)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 3f;
            agent = pAgent;
            damageZone = pDamageZone;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            damageZone.enabled = true;
            agent.speed = CHARGE_SPEED;
            agent.angularSpeed = 5;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            animator.SetFloat("Speed", agent.velocity.magnitude/ CHARGE_SPEED);
            agent.destination = (base.enemy.currentTarget.position);
        }

        public override void OnExit()
        {
            base.OnExit();
            damageZone.enabled = false;
            agent.isStopped = true;
            agent.velocity = agent.velocity / 3;
        }
    }
}
