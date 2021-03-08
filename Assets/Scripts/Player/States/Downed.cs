using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Downed : PlayerState
    {
        public Downed(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetBool("Downed", true);
            //_player.gameObject.SetActive(false);
        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("Downed", false);
        }
    }
}