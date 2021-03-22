using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace hulaohyes.player.states
{
    public abstract class Idle : PlayerState
    {
        //private const float MOVEMENT_SPEED = 6f;
        //private const float JUMP_HEIGHT = 8f;
        const float ROTATION_SMOOTHING_AMOUNT = 0.75f;
        const float DEG2RAD = Mathf.PI/ 180;

        protected PlayerController _player;
        protected ControlScheme _controlScheme;

        private Transform cameraContainer;
        private Rigidbody rb;
        private Vector3 camForward;
        private Vector3 camRight;
        private Vector2 movementInput;
        private LayerMask groundLayer;

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
            cameraContainer = pCameraContainer;
            rb = pRb;

            movementInput = Vector2.zero;
            groundLayer = LayerMask.GetMask("Ground","Bricks");
        }


        ///Rotate player facing last direction
        private void RotatePlayer()
        {
            Vector3 lDesiredDirection = camForward * movementInput.y + camRight * movementInput.x;

            if (movementInput != Vector2.zero)
            {
                Quaternion lDesiredRotation = Quaternion.LookRotation(new Vector3(lDesiredDirection.x, 0, lDesiredDirection.z));
                _player.transform.rotation = Quaternion.Slerp(lDesiredRotation, _player.transform.rotation, ROTATION_SMOOTHING_AMOUNT);
            }
        }

        private void CameraDirection()
        {
            Vector3 lPreCamForward = cameraContainer.transform.forward;
            camForward = Quaternion.AngleAxis(-cameraContainer.eulerAngles.x, cameraContainer.right) * cameraContainer.transform.forward;
            camRight = cameraContainer.transform.right;
        }

        private void MovePlayer()                                                                                                                   //const to change
        {
            Vector3 lDesiredPosition = camForward * movementInput.y + camRight * movementInput.x;
            rb.velocity = new Vector3(lDesiredPosition.x * _player.MOVEMENT_SPEED, rb.velocity.y, lDesiredPosition.z * _player.MOVEMENT_SPEED);   //const to change
            base.animator.SetFloat("Speed", rb.velocity.magnitude / _player.MOVEMENT_SPEED);                                                      //const to change
            base.animator.SetBool("isGrounded", isGrounded);
        }

        private bool isGrounded =>(Physics.Raycast(_player.transform.position, -_player.transform.up, _player.GROUND_CHECK_DISTANCE, groundLayer));

        private void OnJump(InputAction.CallbackContext ctx)                                                                                        //const to change
        { 
            if (isGrounded)
            {
                particles[0].Play();
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
                Vector3 upDir = new Vector3(0, _player.JUMP_HEIGHT, 0);                                                                             //const to change
                rb.velocity = upDir;
            }
        }

        void ParticleManagement()
        {
            if (isGrounded && !particles[1].isPlaying && movementInput.magnitude > 0.1f) particles[1].Play();
            else if (!isGrounded && particles[1].isPlaying) particles[1].Stop();
        }

        void OnMovementsStop(InputAction.CallbackContext ctx)
        {
            if (particles[1].isPlaying) particles[1].Stop();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _controlScheme.Player.Jump.performed += OnJump;
            _controlScheme.Player.Movement.canceled += OnMovementsStop;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            CameraDirection();
            ParticleManagement();
            movementInput = _controlScheme.Player.Movement.ReadValue<Vector2>();
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
            _controlScheme.Player.Movement.canceled -= OnMovementsStop;
            particles[1].Stop();
        }
    }
}
