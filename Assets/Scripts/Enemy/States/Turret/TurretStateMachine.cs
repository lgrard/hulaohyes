using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.turret
{
    public class TurretStateMachine : EnemyStateMachine
    {
        public TurretStateMachine(EnemyController pEnemy, Rigidbody pRb,
        Animator pAnimator, List<ParticleSystem> pParticles, NavMeshAgent pNavMeshAgent, SphereCollider pDetectionZone)
            : base(pEnemy, pRb, pAnimator, pParticles, pNavMeshAgent, pDetectionZone)
        {
            _attacking = new Shooting(this, pEnemy, pAnimator,pNavMeshAgent);
        }
    }
}
