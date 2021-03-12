using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using hulaohyes.levelbrick.unitcube;

namespace hulaohyes.player.states
{
    public class Running : Idle
    {
        //private const float PICK_UP_DISTANCE = 2;
        private Vector3 targettingOffset = new Vector3(0, 0.5f, 0);
        private LayerMask pickableLayers;
        private UnitCubeSpawner currentSpawner;

        /// Create a new running state object
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        public Running(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles)
        { pickableLayers = LayerMask.GetMask("Enemy", "Player", "Bricks"); }

        void OnPickup(InputAction.CallbackContext ctx)
        {
            if (_player.pickUpTarget != null)
            {
                stateMachine.CurrentState = stateMachine.Carrying;
                return;
            }

            else if (currentSpawner != null) currentSpawner.PushButton();
        }

        void Targetting()
        {
            Ray lRay = new Ray(_player.transform.position + targettingOffset, _player.transform.forward);
            RaycastHit hit;

            if(Physics.Raycast(lRay,out hit,_player.PICK_UP_DISTANCE,pickableLayers,QueryTriggerInteraction.Ignore)
                && hit.collider.gameObject != _player.gameObject)
            {
                if (hit.collider.TryGetComponent<Pickable>(out Pickable pickableTarget) && pickableTarget.isPickable)
                    _player.pickUpTarget = pickableTarget;
                else if (hit.collider.TryGetComponent<UnitCubeSpawner>(out UnitCubeSpawner pSpawner))
                    currentSpawner = pSpawner;
            }

            else if(hit.collider == null)
            {
                if(_player.pickUpTarget != null)
                    _player.pickUpTarget = null;

                else if(currentSpawner != null)
                    currentSpawner = null;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            base._controlScheme.Player.PickUp.performed += OnPickup;
            _player.isPickableState = true;
        }

        public override void PhysLoopLogic()
        {
            base.PhysLoopLogic();
            Targetting();
        }


        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.PickUp.performed -= OnPickup;
            _player.isPickableState = false;
        }
    }
}
