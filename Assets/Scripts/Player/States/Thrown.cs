using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.player.states
{
    public class Thrown : PlayerState
    {
        public Thrown(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }


        public void HitObject(Collision pCollision)
        {
            if (pCollision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                enemy.TakeDamage(1);
                enemy.StartCoroutine(enemy.KnockBack(_player.transform));
            }

            _particles[2].Play();
            _stateMachine.CurrentState = _stateMachine.running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Thrown", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Thrown", false);
        }
    }
}
