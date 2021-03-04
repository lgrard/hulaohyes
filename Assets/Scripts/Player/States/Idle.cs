using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace hulaohyes.player.states
{
    public abstract class Idle : PlayerState
    {
        const float JUMP_HEIGHT = 8f;
        const float ROTATION_SMOOTHING_AMOUNT = 0.75f;
        const float MOVEMENT_SPEED = 6f;
        const float DEG2RAD = Mathf.PI/ 180;

        protected PlayerController _player;
        protected ControlScheme _controlScheme;

        private Transform _cameraContainer;
        private Rigidbody _rb;
        private Vector3 _camForward;
        private Vector3 _camRight;
        private Vector2 _movementInput;
        private LayerMask _groundLayer;

        /// Create a new idle state
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pParticles">Associated particle list</param>
        public Idle(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pAnimator, pParticles)
        {
            _player = pPlayer;
            _controlScheme = pControlScheme;
            _cameraContainer = pCameraContainer;
            _rb = pRb;

            _movementInput = Vector2.zero;
            _groundLayer = LayerMask.GetMask("Ground","Bricks");
        }


        ///Rotate player facing last direction
        private void RotatePlayer()
        {
            Vector3 lDesiredDirection = _camForward * _movementInput.y + _camRight * _movementInput.x;

            if (_movementInput != Vector2.zero)
            {
                Quaternion lDesiredRotation = Quaternion.LookRotation(new Vector3(lDesiredDirection.x, 0, lDesiredDirection.z));
                _player.transform.rotation = Quaternion.Slerp(lDesiredRotation, _player.transform.rotation, ROTATION_SMOOTHING_AMOUNT);
            }
        }

        private void CameraDirection()
        {
            float lAngleOffset = _cameraContainer.transform.eulerAngles.x * DEG2RAD;
            Vector3 lDirOffset = new Vector3(0, Mathf.Sin(lAngleOffset), Mathf.Cos(lAngleOffset));

            _camForward = _cameraContainer.transform.forward + lDirOffset;
            _camRight = _cameraContainer.transform.right;
        }

        private void MovePlayer()
        {
            Vector3 lDesiredPosition = _camForward * _movementInput.y + _camRight * _movementInput.x;
            _rb.velocity = new Vector3(lDesiredPosition.x * MOVEMENT_SPEED, _rb.velocity.y, lDesiredPosition.z * MOVEMENT_SPEED);
            base._animator.SetFloat("Speed", _rb.velocity.magnitude / MOVEMENT_SPEED);
            base._animator.SetBool("isGrounded", isGrounded);
        }

        private bool isGrounded =>(Physics.Raycast(_player.transform.position, -_player.transform.up, 0.5f, _groundLayer));

        private void OnJump(InputAction.CallbackContext ctx)
        { 
            if (isGrounded)
            {
                _particles[0].Play();
                _animator.SetTrigger("Jump");
                Vector3 upDir = new Vector3(0, JUMP_HEIGHT, 0);
                _rb.velocity = upDir;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _controlScheme.Player.Jump.performed += OnJump;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            CameraDirection();
            _movementInput = _controlScheme.Player.Movement.ReadValue<Vector2>();
        }

        public override void PhysLoopLogic()
        {
            base.PhysLoopLogic();
            MovePlayer();
            RotatePlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            _controlScheme.Player.Jump.performed -= OnJump;
        }
    }
}
