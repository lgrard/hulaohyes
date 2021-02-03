using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.player
{
    public class PlayerController : MonoBehaviour
    {
        enum State
        {
            idle,
            running,
            attacking,
            carried,
            carring,
            takingDamage,
            dead
        }

        State currentState = State.idle;
        Camera mainCamera;
        Animator playerAnimator;
        Vector2 movementInput = Vector2.zero;
        float rotationSmoothingAmount = 0.01f;

        [Tooltip("Layer(s) used for ground detection")]
        [SerializeField] LayerMask groundLayer;

        //Constructor and initialization
        private void Start()
        {
            mainCamera = Camera.main;
            playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            PlayerStateMachine();
        }

        ///Allows player action depending on the current state
        void PlayerStateMachine()
        {
            switch (currentState)
            {
                case State.idle:
                    break;

                case State.running:;
                    break;

                case State.attacking:
                    break;

                case State.carried:
                    break;

                case State.carring:
                    break;

                case State.takingDamage:
                    break;

                case State.dead:
                    break;
            }
        }

        ///Transition player to a new state
        ///<param name="newState">State to transition to</param>
        State setCurrentState(State newState)
        {
            currentState = newState;
            return currentState;
        }

        ///Rotate player facing last direction
        void RotatePlayer()
        {
            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;
            Vector3 DesiredRotation = camForward * movementInput.y + camRight * movementInput.x;
            Quaternion desiredRotation = Quaternion.LookRotation(new Vector3(DesiredRotation.x, 0, DesiredRotation.z));

            transform.rotation = Quaternion.Slerp(desiredRotation, transform.rotation, rotationSmoothingAmount);
        }

        ///Return if player is grounded
        bool isGrounded => (Physics.Raycast(transform.position, -transform.up, 1, groundLayer));

        ///Destroy player's object and delete references
        public PlayerController destroyPlayer()
        {
            //Destructor
            return this;
        }
    }
}