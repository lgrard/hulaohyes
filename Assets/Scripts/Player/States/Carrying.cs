using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carrying : Idle
    {
        private const int HIT_BEFORE_DROP = 1;
        private int currentHit;

        public Carrying(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) {}

        public void TakeCarryDamage()
        {
            animator.SetTrigger("TakeDamage");
            currentHit -= 1;

            if (currentHit <= 0)
            {
                _player.pickUpTarget.Drop();
                stateMachine.CurrentState = stateMachine.Wait;
            }
        }

        private void PickUpTarget()
        {
            if(_player.pickUpTarget.isPickable) _player.pickUpTarget.GetPicked(_player);
            currentHit = HIT_BEFORE_DROP;
        }

        private void OnDropTarget(InputAction.CallbackContext ctx) => _player.pickUpTarget.Drop();

        private void OnThrowTarget(InputAction.CallbackContext ctx) => _player.pickUpTarget.Propel();

        public override void OnEnter()
        {
            currentHit = HIT_BEFORE_DROP;
            base.OnEnter();
            PickUpTarget();
            base._controlScheme.Player.Drop.performed += OnDropTarget;
            base._controlScheme.Player.PickUp.performed += OnThrowTarget;
            animator.SetBool("Carrying", true);
            animator.ResetTrigger("Throw");
            _player.isPickableState = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.Drop.performed -= OnDropTarget;
            base._controlScheme.Player.PickUp.performed -= OnThrowTarget;
            animator.SetBool("Carrying", false);
            animator.ResetTrigger("Throw");
            _player.isPickableState = false;
        }
    }
}