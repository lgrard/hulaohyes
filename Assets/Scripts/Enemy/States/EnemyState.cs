using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public abstract class EnemyState : State
    {
        protected EnemyStateMachine _stateMachine;
        protected EnemyController _enemy;
        protected Animator _animator;

        /// Create a new state
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pEnemy">Associated player controller</param>
        /// <param name="pAnimator">Associated animator component</param>
        public EnemyState(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base()
        {
            _stateMachine = pStateMachine;
            _enemy = pEnemy;
            _animator = pAnimator;
        }
    }
}
