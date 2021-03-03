using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carried : Idle
    {
        public Carried(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }

        void OnDrop(InputAction.CallbackContext ctx) => _player.Drop();

        protected override void Jump()
        {
            _player.Drop();
            base.Jump();
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