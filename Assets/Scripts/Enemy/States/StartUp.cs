using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states;
using hulaohyes.enemy;
using UnityEngine.AI;

public class StartUp : Wait
{
    private NavMeshAgent agent;

    public StartUp(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent)
        :base(pStateMachine, pEnemy, pAnimator)
    {
        agent = pAgent;
        MAX_TIMER = 3f;
    }

    protected override void TimerEnd()
    {
        base.TimerEnd();
        stateMachine.CurrentState = stateMachine.Attacking;
    }

    public override void LoopLogic()
    {
        base.LoopLogic();
        agent.destination = (enemy.currentTarget.transform.position);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = false;
        agent.speed = 0.1f;
        agent.angularSpeed = 120;
        animator.SetBool("Attacking", true);
    }
}
