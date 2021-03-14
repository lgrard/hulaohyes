﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy.states
{
    public class Thrown : Wait
    {
        public Thrown(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base(pStateMachine, pEnemy, pAnimator) { MAX_TIMER = 5f; }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            enemy.destroyEnemy();
        }
    }
}
