using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carrying : Idle
    {
        private const int HIT_BEFORE_DROP = 3;
        private int currentHit;

        public Carrying(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) {}

        public void TakeCarryDamage()
        {
            _animator.SetTrigger("TakeDamage");
            currentHit -= 1;

            if (currentHit <= 0)
            {
                _player.Drop();
                _stateMachine.CurrentState = _stateMachine.TakingDamage;
            }
        }

        void PickUpTarget()
        {
            if(_player.pickUpTarget.isPickable) _player.pickUpTarget.GetPicked(_player);
            currentHit = HIT_BEFORE_DROP;
        }

        void OnDropTarget(InputAction.CallbackContext ctx) => _player.pickUpTarget.Drop();

        void OnThrowTarget(InputAction.CallbackContext ctx) => _player.pickUpTarget.Propel();

        protected override void Jump()
        {
            if (isGrounded) base.Jump();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PickUpTarget();
            base._controlScheme.Player.PickUp.performed += OnDropTarget;
            base._controlScheme.Player.Punch.performed += OnThrowTarget;
            _animator.SetBool("Carrying", true);
            _animator.ResetTrigger("Throw");
        }

        public override void PhysLoopLogic()
        {
            base.PhysLoopLogic();
            base.MovePlayer();
            base.RotatePlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.PickUp.performed -= OnDropTarget;
            base._controlScheme.Player.Punch.performed -= OnThrowTarget;
            _animator.SetBool("Carrying", false);
            _animator.ResetTrigger("Throw");
        }
    }
}