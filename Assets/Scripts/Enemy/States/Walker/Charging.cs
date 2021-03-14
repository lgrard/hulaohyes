using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.walker
{
    public class Charging : Attacking
    {
        //private const float CHARGE_DELAY = 3f;                                         
        //private const float CHARGE_ROTATION_AMOUNT = 0.01f;
        //private const float CHARGE_SPEED = 6;

        private NavMeshAgent agent;
        private BoxCollider damageZone;

        public Charging(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent, BoxCollider pDamageZone, List<ParticleSystem> pParticles)
            : base(pStateMachine, pEnemy, pAnimator, pParticles)
        {
            MAX_TIMER = enemy.CHARGE_DURATION;                                 //const to change
            agent = pAgent;
            damageZone = pDamageZone;
        }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            particles[1].Play();
            agent.velocity = Vector3.zero;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            particles[0].Play();
            damageZone.enabled = true;
            agent.isStopped = false;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();

            Vector3 lDirection = enemy.currentTarget.position - enemy.transform.position;
            Quaternion lRotation = Quaternion.LookRotation(lDirection, Vector3.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lRotation, enemy.CHARGE_ROTATION_AMOUNT*Time.deltaTime);                                     //const to change
            agent.velocity = new Vector3(enemy.transform.forward.x * enemy.CHARGE_SPEED, agent.velocity.y, enemy.transform.forward.z * enemy.CHARGE_SPEED);     // const to change
        }

        public override void OnExit()
        {
            base.OnExit();
            particles[0].Stop();
            damageZone.enabled = false;
            agent.isStopped = true;
            agent.velocity = agent.velocity / 3;
        }
    }
}
