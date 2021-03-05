using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;
using hulaohyes.enemy.states;

public abstract class Wait : EnemyState
{
    protected float MAX_TIMER = 0.4f;
    private float _timer;

    public Wait(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator) : base(pStateMachine, pEnemy, pAnimator) { }

    protected virtual void TimerEnd() { }

    public override void OnEnter()
    {
        base.OnEnter();
        _timer = MAX_TIMER;
    }

    public override void LoopLogic()
    {
        base.LoopLogic();
        if (_timer >= 0) _timer -= Time.deltaTime;
        else TimerEnd();
    }

}
