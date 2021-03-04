﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;
using hulaohyes.enemy.states;

public class Recovering : Wait
{
    public Recovering(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
    : base(pStateMachine, pEnemy, pAnimator)
    { MAX_TIMER = 3f; }

    protected override void TimerEnd()
    {
        base.TimerEnd();
        _stateMachine.CurrentState = _stateMachine.Idle;
    }

}
