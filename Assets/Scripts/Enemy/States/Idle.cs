using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public class Idle : EnemyState
    {
        private SphereCollider detectionZone;
        private LayerMask playerLayer;

        public Idle(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, SphereCollider pDetectionZone)
            : base(pStateMachine,pEnemy,pAnimator)
        {
            detectionZone = pDetectionZone;
            playerLayer = LayerMask.GetMask("Player");
        }

        private void PlayerCheck()
        {
            enemy.currentTarget = Utility.GetClosestTarget(enemy.transform, Physics.OverlapSphere(enemy.transform.position, detectionZone.radius, playerLayer));
            if(enemy.currentTarget != null)stateMachine.CurrentState = stateMachine.StartUp;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter Idle");
            base.OnEnter();
            detectionZone.enabled = true;
            PlayerCheck();
        }

        public override void OnExit()
        {
            Debug.Log("Exit Idle");
            base.OnExit();
            detectionZone.enabled = false;
        }
    }
}
