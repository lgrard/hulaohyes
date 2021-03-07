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
        private int _currentHit;

        public Carrying(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) {}

        public void TakeCarryDamage()
        {
            _animator.SetTrigger("TakeDamage");
            _currentHit -= 1;

            if (_currentHit <= 0)
            {
                _player.Drop();
                _stateMachine.CurrentState = _stateMachine.Wait;
            }
        }

        private void PickUpTarget()
        {
            if(_player.pickUpTarget.isPickable) _player.pickUpTarget.GetPicked(_player);
            _currentHit = HIT_BEFORE_DROP;
        }

        private void OnDropTarget(InputAction.CallbackContext ctx) => _player.pickUpTarget.Drop();

        private void OnThrowTarget(InputAction.CallbackContext ctx) => _player.pickUpTarget.Propel();

        public override void OnEnter()
        {
            base.OnEnter();
            PickUpTarget();
            base._controlScheme.Player.Drop.performed += OnDropTarget;
            base._controlScheme.Player.PickUp.performed += OnThrowTarget;
            _animator.SetBool("Carrying", true);
            _animator.ResetTrigger("Throw");
            _player.isPickableState = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.Drop.performed -= OnDropTarget;
            base._controlScheme.Player.PickUp.performed -= OnThrowTarget;
            _animator.SetBool("Carrying", false);
            _animator.ResetTrigger("Throw");
            _player.isPickableState = false;
        }
    }
}