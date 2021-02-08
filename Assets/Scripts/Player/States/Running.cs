using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Running : Idle
    {
        const float PICK_UP_DISTANCE = 2;
        private LayerMask _pickableLayers;

        /// Create a new running state object
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        public Running(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator) => _pickableLayers = LayerMask.GetMask("Enemy", "Player");


        void Punch(InputAction.CallbackContext ctx) => _stateMachine.CurrentState = _stateMachine.attacking;

        void Pickup(InputAction.CallbackContext ctx)
        {
            if (_player.pickUpTarget != null) _stateMachine.CurrentState = _stateMachine.carrying;
        }

        void Targetting()
        {
            Ray lFrontRay = new Ray(_player.transform.position,_player.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(lFrontRay,out hit, PICK_UP_DISTANCE, _pickableLayers)
                && hit.collider.gameObject != _player && hit.collider.gameObject != _player.pickUpTarget)
            {
                _player.pickUpTarget = hit.collider.GetComponent<Pickable>();
                Debug.Log("Current target: " + _player.pickUpTarget.gameObject.name);
            }

            else if(hit.collider == null && _player.pickUpTarget != null)
            {
                _player.pickUpTarget = null;
                Debug.Log("Current target: null");
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            base._controlScheme.Player.Punch.performed += Punch;
            base._controlScheme.Player.PickUp.performed += Pickup;
        }

        public override void PhysLoopLogic()
        {
            base.PhysLoopLogic();
            Targetting();
        }

        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.Punch.performed -= Punch;
            base._controlScheme.Player.PickUp.performed -= Pickup;
        }
    }
}
