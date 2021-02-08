using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Attacking : PlayerState
    {
        const float MAX_ATTACK_STAMP = 0.5f;
        const float ATTACK_RADIUS = 1f;

        private LayerMask _enemyLayer;
        private float _attackStamp;
        private Transform _attackPoint;

        /// Create a new attacking state object
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pAttackPoint">Hit collision transform</param>
        public Attacking(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, Transform pAttackPoint)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator)
        {
            _attackPoint = pAttackPoint;
            _enemyLayer = LayerMask.GetMask("Enemy");
        }


        Collider[] MagnetCheck()
        {
            Collider[] lHitColliders = Physics.OverlapSphere(_attackPoint.position, ATTACK_RADIUS, _enemyLayer);
            return lHitColliders;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _attackStamp = MAX_ATTACK_STAMP;
            MagnetCheck();
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (_attackStamp > 0) _attackStamp -= Time.deltaTime;
            else base._stateMachine.CurrentState = _stateMachine.running;
        }
    }
}

