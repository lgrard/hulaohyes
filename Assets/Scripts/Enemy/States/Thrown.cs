using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;
using hulaohyes.enemy.states;

public class Thrown : Wait
{
    public Thrown(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
        : base(pStateMachine, pEnemy, pAnimator) { MAX_TIMER = 5f; }

    protected override void TimerEnd()
    {
        base.TimerEnd();
        _enemy.destroyEnemy();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _animator.SetBool("Thrown", true);
    }

    public override void OnExit()
    {
        base.OnExit();
        _animator.SetBool("Thrown", false);
    }
}
