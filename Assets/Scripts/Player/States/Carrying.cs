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
        private Transform _pickUpPoint;

        public Carrying(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles, Transform pPickUpPoint)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles) { _pickUpPoint = pPickUpPoint; }

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

        public IEnumerator DropTarget(bool pDrop)
        {
            _animator.SetBool("Carrying", false);
            base._controlScheme.Player.PickUp.performed -= OnDropTarget;
            base._controlScheme.Player.Punch.performed -= OnThrowTarget;
            yield return new WaitForSeconds(0.1f);
            if (pDrop) _player.pickUpTarget.CurrentPicker = null;          
            _stateMachine.CurrentState = _stateMachine.running;
        }

        void PickUpTarget()
        {
            Debug.Log("You have picked: " + _player.pickUpTarget.name);
            _player.pickUpTarget.transform.parent = _pickUpPoint;
            _player.pickUpTarget.CurrentPicker = _player;

            _animator.SetBool("Carrying", true);
            currentHit = HIT_BEFORE_DROP;
        }

        void OnDropTarget(InputAction.CallbackContext ctx) => _player.StartCoroutine(DropTarget(true));

        void OnThrowTarget(InputAction.CallbackContext ctx)
        {
            _player.pickUpTarget.Propel();
            _stateMachine.CurrentState = _stateMachine.running;
        }
        protected override void Jump()
        {
            if (isGrounded) base.Jump();
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