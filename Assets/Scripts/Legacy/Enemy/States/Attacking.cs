using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public class Attacking : Wait
    {
        protected List<ParticleSystem> particles;

        public Attacking(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pEnemy, pAnimator)
        { particles = pParticles; }

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
