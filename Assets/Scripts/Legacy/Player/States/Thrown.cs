using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.player.states
{
    public class Thrown : Wait
    {
        public Thrown(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pAnimator, pParticles)
        { MAX_TIMER = 3; }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetBool("Thrown", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("Thrown", false);
        }
    }
}
