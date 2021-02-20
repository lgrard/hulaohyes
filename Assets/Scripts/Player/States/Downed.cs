using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Downed : PlayerState
    {
        public Downed(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Downed", true);
            //_player.gameObject.SetActive(false);
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Downed", false);
        }
    }
}