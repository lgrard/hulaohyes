using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.walker
{
    public class WalkerStateMachine : EnemyStateMachine
    {
        public WalkerStateMachine(EnemyController pEnemy, Rigidbody pRb,
        Animator pAnimator, List<ParticleSystem> pParticles, NavMeshAgent pNavMeshAgent, SphereCollider pDetectionZone, BoxCollider pDamageZone)
            :base(pEnemy,pRb,pAnimator,pParticles,pDetectionZone)
        {
            _attacking = new Charging(this, pEnemy, pAnimator, pNavMeshAgent, pDamageZone, pParticles, pRb);
            _recovering = new RecoveringWalker(this, pEnemy, pAnimator, pRb);
        }
    }
}
