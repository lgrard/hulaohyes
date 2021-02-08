using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carrying : Idle
    {
        const float THROW_FORCE = 500;
        private Vector3 _pickUpPoint = new Vector3(0, 2.5f, 0);

        public Carrying(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator) { }

        void PickUpTarget()
        {
            Debug.Log("You have picked: " + _player.pickUpTarget.name);

            _player.pickUpTarget.CurrentPicker = _player;
            _player.pickUpTarget.transform.localPosition = _pickUpPoint;
        }

        void DropTarget(InputAction.CallbackContext ctx)
        {
            _player.pickUpTarget.CurrentPicker = null;
            _stateMachine.CurrentState = _stateMachine.running;
        }
        void ThrowTarget(InputAction.CallbackContext ctx)
        {
            _player.pickUpTarget.CurrentPicker = null;
            _player.pickUpTarget.GetComponent<Rigidbody>().AddForce(_player.transform.forward * THROW_FORCE);
            _stateMachine.CurrentState = _stateMachine.running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PickUpTarget();
            base._controlScheme.Player.PickUp.performed += DropTarget;
            base._controlScheme.Player.Punch.performed += ThrowTarget;
        }

        public override void OnExit()
        {
            base.OnExit();
            base._controlScheme.Player.PickUp.performed -= DropTarget;
            base._controlScheme.Player.Punch.performed -= ThrowTarget;
        }
    }
}