using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;
using hulaohyes.enemy.states;

public class RecoveringWalker : Recovering
{
    public RecoveringWalker(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
    : base(pStateMachine, pEnemy, pAnimator)
    { MAX_TIMER = 3f; }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.isPickableState = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.isPickableState = false;
    }
}
