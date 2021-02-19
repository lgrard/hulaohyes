using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.states;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public abstract class EnemyState : State
    {
        protected EnemyStateMachine _stateMachine;
        protected EnemyController _enemy;
        protected Rigidbody _rb;
        protected Animator _animator;
        protected List<ParticleSystem> _particles;
        protected NavMeshAgent _navMeshAgent;

        /// Create a new state
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pEnemy">Associated player controller</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pParticles">Associated particles list</param>
        /// <param name="pNavMeshAgent">Associated Navmesh agent</param>
        public EnemyState(EnemyStateMachine pStateMachine, EnemyController pEnemy,
            Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles, NavMeshAgent pNavMeshAgent)
            : base()
        {
            _stateMachine = pStateMachine;
            _enemy = pEnemy;
            _rb = pRb;
            _animator = pAnimator;
            _particles = pParticles;
            _navMeshAgent = pNavMeshAgent;
        }
    }
}
