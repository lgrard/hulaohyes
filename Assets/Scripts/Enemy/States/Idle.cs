using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public class Idle : EnemyState
    {
        public Idle(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base(pStateMachine,pEnemy,pAnimator)
        { }
    }
}
