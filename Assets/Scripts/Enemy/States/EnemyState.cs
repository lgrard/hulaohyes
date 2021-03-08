using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public abstract class EnemyState : State
    {
        protected EnemyStateMachine stateMachine;
        protected EnemyController enemy;
        protected Animator animator;

        /// Create a new state
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pEnemy">Associated player controller</param>
        /// <param name="pAnimator">Associated animator component</param>
        public EnemyState(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base()
        {
            stateMachine = pStateMachine;
            enemy = pEnemy;
            animator = pAnimator;
        }
    }
}
