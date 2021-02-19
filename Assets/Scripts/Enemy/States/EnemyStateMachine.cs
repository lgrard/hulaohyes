using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.states;

namespace hulaohyes.enemy.states
{
    public class EnemyStateMachine : StateMachine
    {
        ///Create a new state machine object
        /// <param name="pEnemy">Associated player controller</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pAttackPoint"></param>
        /// <param name="pParticles">Associated particles list</param>
        /// <param name="pNavMeshAgent">Associated NavMesh agent</param>
        public EnemyStateMachine(EnemyController pEnemy, Rigidbody pRb,
            Animator pAnimator, Transform pAttackPoint, List<ParticleSystem> pParticles, NavMeshAgent pNavMeshAgent)
        {
            //CurrentState = ;
        }
    }
}

