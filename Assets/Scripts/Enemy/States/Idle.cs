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
            Transform lNearestTarget = null;
            float lNearestDistance = 0;

            foreach (Collider lTarget in Physics.OverlapSphere(_enemy.transform.position, _detectionZone.radius, _playerLayer))
            {
                Debug.Log(lTarget.gameObject.name + "is in range");

                float lTargetDistance = Vector3.Distance(_enemy.transform.position, lTarget.transform.position);

                if (lNearestTarget == null)
                {
                    lNearestTarget = lTarget.transform;
                    lNearestDistance = lTargetDistance;
                }
                else if (lNearestTarget != null && lTargetDistance < lNearestDistance) lNearestTarget = lTarget.transform;
            }

            _enemy.currentTarget = lNearestTarget;

            if(_enemy.currentTarget != null) _stateMachine.CurrentState = _stateMachine.StartUp;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PlayerCheck();
            _detectionZone.enabled = true;
        }

        public override void OnExit()
        {
            base.OnEnter();
            _detectionZone.enabled = false;
        }
    }
}
