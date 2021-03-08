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
    { MAX_TIMER = 5f; }


    protected override void TimerEnd()
    {
        base.TimerEnd();
        enemy.Drop();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.SetBool("Carried", true);
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetBool("Carried", false);
    }
}
