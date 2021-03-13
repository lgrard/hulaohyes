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

        private PlayerController player;
        private ControlScheme controlScheme;
        private Rigidbody rb;
        private Collider collider;

        public Carried(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles, Collider pCollider)
            : base(pStateMachine, pAnimator, pParticles)
        {
            player = pPlayer;
            controlScheme = pControlScheme;
            rb = pRb;
            collider = pCollider;
        }

        private void OnDrop(InputAction.CallbackContext ctx) => player.Drop();

        private void OnJump(InputAction.CallbackContext ctx)
        {
            player.JumpFromCarried();

            particles[0].Play();
            animator.SetTrigger("Jump");
            Vector3 upDir = new Vector3(0, player.VERTICAL_PROPEL_HEIGHT, 0);                                                                                  //const to change
            rb.velocity = upDir;

            collider.isTrigger = false;
            stateMachine.CurrentState = stateMachine.Running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetBool("Carried", true);
            controlScheme.Player.Jump.performed += OnJump;
            controlScheme.Player.PickUp.performed += OnDrop;
            controlScheme.Player.Drop.performed += OnDrop;
        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("Carried", false);
            controlScheme.Player.Jump.performed -= OnJump;
            controlScheme.Player.PickUp.performed -= OnDrop;
            controlScheme.Player.Drop.performed -= OnDrop;
        }
    }
}