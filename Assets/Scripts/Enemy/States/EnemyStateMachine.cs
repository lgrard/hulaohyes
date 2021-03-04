using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.states;

namespace hulaohyes.enemy.states
{
    public class EnemyStateMachine : StateMachine
    {
        private Carried _carried;
        private Attacking _attacking;
        private Idle _idle;
        private Recovering _recovering;
        private Thrown _thrown;

        ///Create a new state machine object
        /// <param name="pEnemy">Associated player controller</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pAttackPoint"></param>
        /// <param name="pParticles">Associated particles list</param>
        /// <param name="pNavMeshAgent">Associated NavMesh agent</param>
        public EnemyStateMachine(EnemyController pEnemy, Rigidbody pRb,
            Animator pAnimator, List<ParticleSystem> pParticles, NavMeshAgent pNavMeshAgent)
        {
            _carried = new Carried(this, pEnemy, pAnimator);
            _attacking = new Attacking(this, pEnemy, pAnimator, pNavMeshAgent);
            _idle = new Idle(this, pEnemy, pAnimator);
            _thrown = new Thrown(this, pEnemy, pAnimator);
            _recovering = new Recovering(this, pEnemy, pAnimator);
            CurrentState = _idle;
        }

        public Carried Carried => _carried;
        public Attacking Attacking => _attacking;
        public Idle Idle => _idle;
        public Recovering Recovering => _recovering;
        public Thrown Thrown => _thrown;
    }
}

