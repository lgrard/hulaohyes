using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carried : Idle
    {
        private Collider _collider;

        public Carried(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles, Collider pCollider)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { _collider = pCollider; }

        void OnDrop(InputAction.CallbackContext ctx) => _player.Drop();

        protected override void Jump()
        {
            _player.Drop();
            base.Jump();
            _collider.isTrigger = false;
            _stateMachine.CurrentState = _stateMachine.Running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Carried", true);
            base._controlScheme.Player.PickUp.performed += OnDrop;
            base._controlScheme.Player.Punch.performed += OnDrop;
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Carried", false);
            base._controlScheme.Player.PickUp.performed -= OnDrop;
            base._controlScheme.Player.Punch.performed -= OnDrop;
        }
    }
}