using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.turret
{
    public class Shooting : Attacking
    {
        public Shooting(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, List<ParticleSystem> pParticles)
            : base(pStateMachine, pEnemy, pAnimator, pParticles)
        { MAX_TIMER = 1f; }
    }
}
