using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;
using hulaohyes.enemy.states;
using UnityEngine.AI;

public class Carried : Wait
{
    public Carried(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
        : base(pStateMachine, pEnemy, pAnimator)
    { MAX_TIMER = 10f; }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void TimerEnd()
    {
        base.TimerEnd();
        _stateMachine.CurrentState = _stateMachine.Idle;
        _enemy.Drop();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
