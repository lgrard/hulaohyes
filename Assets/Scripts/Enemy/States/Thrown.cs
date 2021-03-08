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
        enemy.destroyEnemy();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.SetBool("Thrown", true);
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetBool("Thrown", false);
    }
}
