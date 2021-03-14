using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public class StartUp : Wait
    {
        private List<ParticleSystem> particles;
        //private const float STARTUP_ROTATION_AMOUNT = 0.01f;
        //private const float START_UP_DURATION = 3f;

        public StartUp(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, List<ParticleSystem> pParticles )
            : base(pStateMachine, pEnemy, pAnimator)
        {
            particles = pParticles;
            MAX_TIMER = enemy.START_UP_DURATION;
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
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lRotation, enemy.STARTUP_ROTATION_AMOUNT*Time.deltaTime);     //const to change
        }

        public override void OnEnter()
        {
            base.OnEnter();
            particles[2].Play();
            animator.SetBool("Attacking", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            particles[2].Stop();
        }
    }
}
