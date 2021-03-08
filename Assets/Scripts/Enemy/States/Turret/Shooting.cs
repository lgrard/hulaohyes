using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.turret
{
    public class Shooting : Attacking
    {
        private NavMeshAgent agent;

        public Shooting(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 1f;
            agent = pAgent;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Shoot");
        }

        public override void OnExit()
        {
            base.OnExit();
            agent.isStopped = true;
        }
    }
}
