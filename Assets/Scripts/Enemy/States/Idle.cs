using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public class Idle : EnemyState
    {
        private SphereCollider _detectionZone;
        private LayerMask _playerLayer;

        public Idle(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, SphereCollider pDetectionZone)
            : base(pStateMachine,pEnemy,pAnimator)
        {
            _detectionZone = pDetectionZone;
            _playerLayer = LayerMask.GetMask("Player");
        }

        private void PlayerCheck()
        {
            _enemy.currentTarget = Utility.GetClosestTarget(_enemy.transform, Physics.OverlapSphere(_enemy.transform.position, _detectionZone.radius, _playerLayer));
            if(_enemy.currentTarget != null)_stateMachine.CurrentState = _stateMachine.StartUp;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter Idle");
            base.OnEnter();
            _detectionZone.enabled = true;
            PlayerCheck();
        }

        public override void OnExit()
        {
            Debug.Log("Exit Idle");
            base.OnExit();
            _detectionZone.enabled = false;
        }
    }
}
