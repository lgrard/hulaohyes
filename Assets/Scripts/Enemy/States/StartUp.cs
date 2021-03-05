using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states;
using hulaohyes.enemy;
using UnityEngine.AI;

public class StartUp : Wait
{
    private NavMeshAgent _agent;

    public StartUp(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent)
        :base(pStateMachine, pEnemy, pAnimator)
    {
        _agent = pAgent;
        MAX_TIMER = 3f;
    }

    protected override void TimerEnd()
    {
        base.TimerEnd();
        _stateMachine.CurrentState = _stateMachine.Attacking;
    }

    public override void LoopLogic()
    {
        base.LoopLogic();
        _agent.destination = (_enemy.currentTarget.transform.position);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.isStopped = false;
        _agent.speed = 0.1f;
        _agent.angularSpeed = 120;
    }
}
