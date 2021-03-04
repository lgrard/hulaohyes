using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carried : PlayerState
    {
        //private const float VERTICAL_PROPEL_HEIGHT = 8f;

        private PlayerController _player;
        private ControlScheme _controlScheme;
        private Rigidbody _rb;
        private Collider _collider;

        public Carried(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles, Collider pCollider)
            : base(pStateMachine, pAnimator, pParticles)
        {
            _player = pPlayer;
            _controlScheme = pControlScheme;
            _rb = pRb;
            _collider = pCollider;
        }

        private void OnDrop(InputAction.CallbackContext ctx) => _player.Drop();

        private void OnJump(InputAction.CallbackContext ctx)
        {
            _player.Drop();

            _particles[0].Play();
            _animator.SetTrigger("Jump");
            Vector3 upDir = new Vector3(0, _player.VERTICAL_PROPEL_HEIGHT, 0);                                                                                  //const to change
            _rb.velocity = upDir;

            _collider.isTrigger = false;
            _stateMachine.CurrentState = _stateMachine.Running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _animator.SetBool("Carried", true);
            _controlScheme.Player.Jump.performed += OnJump;
            _controlScheme.Player.PickUp.performed += OnDrop;
            _controlScheme.Player.Punch.performed += OnDrop;
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Carried", false);
            _controlScheme.Player.Jump.performed -= OnJump;
            _controlScheme.Player.PickUp.performed -= OnDrop;
            _controlScheme.Player.Punch.performed -= OnDrop;
        }
    }
}