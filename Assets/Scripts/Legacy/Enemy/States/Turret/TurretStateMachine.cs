using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.turret
{
    public class TurretStateMachine : EnemyStateMachine
    {
        public TurretStateMachine(EnemyController pEnemy, Rigidbody pRb,
        Animator pAnimator, List<ParticleSystem> pParticles, SphereCollider pDetectionZone)
            : base(pEnemy, pRb, pAnimator, pParticles, pDetectionZone)
        {
            _attacking = new Shooting(this, pEnemy, pAnimator, pParticles);
            _recovering = new Recovering(this, pEnemy, pAnimator);
        }
    }
}
