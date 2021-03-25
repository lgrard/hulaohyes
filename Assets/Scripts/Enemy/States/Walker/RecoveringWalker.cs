using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy;
using hulaohyes.enemy.states;

public class RecoveringWalker : Recovering
{
    private Rigidbody rb;

    public RecoveringWalker(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, Rigidbody pRb)
    : base(pStateMachine, pEnemy, pAnimator)
    {
        MAX_TIMER = 3f;
        rb = pRb;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        rb.isKinematic = true;
        enemy.isPickableState = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.isPickableState = false;
    }
}
