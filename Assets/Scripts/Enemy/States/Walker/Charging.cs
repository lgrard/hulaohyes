using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.walker
{
    public class Charging : Attacking
    {
        private const float CHARGE_ROTATION_AMOUNT = 0.01f;
        private const float CHARGE_SPEED = 6;

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
            agent.isStopped = false;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();

            Vector3 lDirection = enemy.currentTarget.position - enemy.transform.position;
            Quaternion lRotation = Quaternion.LookRotation(lDirection, Vector3.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lRotation, CHARGE_ROTATION_AMOUNT);
            agent.velocity = new Vector3(enemy.transform.forward.x * CHARGE_SPEED, agent.velocity.y, enemy.transform.forward.z * CHARGE_SPEED);
            animator.SetFloat("Speed", agent.velocity.magnitude/ CHARGE_SPEED);
        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetFloat("Speed", 0);
            damageZone.enabled = false;
            agent.isStopped = true;
            agent.velocity = agent.velocity / 3;
        }
    }
}
