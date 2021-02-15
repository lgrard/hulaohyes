using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.player.states
{
    public class Carrying : Idle
    {
        private const int HIT_BEFORE_DROP = 3;
        private int currentHit;
        private Vector3 _pickUpPoint = new Vector3(0, 2.1f, 0);

        public Carrying(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { }

        public void TakeCarryDamage()
        {
            _animator.SetTrigger("TakeDamage");
            currentHit -= 1;

            if(currentHit <= 0)
            {
                _player.pickUpTarget.CurrentPicker = null;
                _stateMachine.CurrentState = _stateMachine.takingDamage;
            }
        }

        void PickUpTarget()
        {
            Debug.Log("You have picked: " + _player.pickUpTarget.name);
            _player.pickUpTarget.CurrentPicker = _player;
            _player.pickUpTarget.transform.localPosition = _pickUpPoint;

            _animator.SetBool("Carrying", true);
            currentHit = HIT_BEFORE_DROP;
        }

        void OnDropTarget(InputAction.CallbackContext ctx)
        {
            _player.pickUpTarget.CurrentPicker = null;
            _stateMachine.CurrentState = _stateMachine.running;
        }
        void OnThrowTarget(InputAction.CallbackContext ctx)
        {
            _player.pickUpTarget.Propel();
            _stateMachine.CurrentState = _stateMachine.running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PickUpTarget();
            base._controlScheme.Player.PickUp.performed += OnDropTarget;
            base._controlScheme.Player.Punch.performed += OnThrowTarget;
        }

        public override void PhysLoopLogic()
        {
            base.PhysLoopLogic();
            base.MovePlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            _animator.SetBool("Carrying", false);
            base._controlScheme.Player.PickUp.performed -= OnDropTarget;
            base._controlScheme.Player.Punch.performed -= OnThrowTarget;
        }
    }
}