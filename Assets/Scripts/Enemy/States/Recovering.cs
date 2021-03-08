using System.Collections;
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
        stateMachine.CurrentState = stateMachine.Idle;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.isPickableState = true;
        animator.SetBool("isRecovering", true);
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.isPickableState = false;
        animator.SetBool("isRecovering", false);
    }
}
