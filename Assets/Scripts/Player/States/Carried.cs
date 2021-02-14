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

        void OnDropSelf(InputAction.CallbackContext ctx)
        {
            _player.CurrentPicker = null;
            _stateMachine.CurrentState = _stateMachine.running;
        }

        void OnThrowSelf(InputAction.CallbackContext ctx)
        {
            _player.Propel();
            _stateMachine.CurrentState = _stateMachine.running;
        }
        protected override void Jump()
        {
            _player.CurrentPicker = null;
            base.Jump();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Carried", true);
            base._controlScheme.Player.PickUp.performed += OnDropSelf;
            base._controlScheme.Player.PickUp.performed += OnThrowSelf;
        }

        public override void OnExit()
        {
            base.OnEnter();
            _animator.SetBool("Carried", false);
            base._controlScheme.Player.PickUp.performed -= OnDropSelf;
            base._controlScheme.Player.PickUp.performed -= OnThrowSelf;
        }
    }
}