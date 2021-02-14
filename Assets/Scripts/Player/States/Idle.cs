using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace hulaohyes.player.states
{
    public abstract class Idle : PlayerState
    {
        const float ROTATION_SMOOTHING_AMOUNT = 0.75f;
        const float MOVEMENT_SPEED = 8f;
        const float JUMP_HEIGHT = 16000f;

        private Vector3 _camForward;
        private Vector3 _camRight;
        private Vector2 _movementInput = Vector2.zero;
        private LayerMask _groundLayer;

        /// Create a new idle state
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        public Idle(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) => _groundLayer = LayerMask.GetMask("Ground");


        ///Rotate player facing last direction
        void RotatePlayer()
        {
            Vector3 lDesiredDirection = _camForward * _movementInput.y + _camRight * _movementInput.x;

            if (_movementInput != Vector2.zero)
            {
                Quaternion lDesiredRotation = Quaternion.LookRotation(new Vector3(lDesiredDirection.x, 0, lDesiredDirection.z));
                base._player.transform.rotation = Quaternion.Slerp(lDesiredRotation, base._player.transform.rotation, ROTATION_SMOOTHING_AMOUNT);
            }
        }

        /// Move player's rigidbody
        void MovePlayer()
        {
            Vector3 lDesiredPosition = _camForward * _movementInput.y + _camRight * _movementInput.x;
            base._rb.velocity = new Vector3(lDesiredPosition.x * MOVEMENT_SPEED, base._rb.velocity.y, lDesiredPosition.z * MOVEMENT_SPEED);
            base._animator.SetFloat("Speed", _rb.velocity.magnitude / MOVEMENT_SPEED);
            base._animator.SetBool("isGrounded", isGrounded);
        }

        ///Return if player is grounded
        protected bool isGrounded =>(Physics.Raycast(base._player.transform.position, -base._player.transform.up, 0.5f, _groundLayer));

        /// Makes the player jump
        protected void OnJump(InputAction.CallbackContext ctx)
        {
            if (isGrounded)
            {
                _particles[0].Play();
                Vector3 upDir = new Vector3(0, JUMP_HEIGHT, 0);
                base._rb.AddForce(upDir);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            base._controlScheme.Player.Jump.performed += OnJump;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();

            _movementInput = _controlScheme.Player.Movement.ReadValue<Vector2>();
            _camForward = base._cameraContainer.transform.forward;
            _camRight = base._cameraContainer.transform.right;

            RotatePlayer();
        }

        public override void PhysLoopLogic()
        {
            base.LoopLogic();

            MovePlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.Jump.performed -= OnJump;
        }
    }
}
