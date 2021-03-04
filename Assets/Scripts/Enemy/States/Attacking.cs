using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.enemy;

namespace hulaohyes.enemy.states
{
    public class Attacking : Wait
    {
        private NavMeshAgent _agent;

        public Attacking(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, NavMeshAgent pAgent)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 10f;
            _agent = pAgent;
        }

        private IEnumerator ChargePlayer()
        {
            yield return new WaitForSeconds(1f);
            _agent.SetDestination(base._enemy.currentTarget.position);
        }

        protected override void TimerEnd()
        {
            base.TimerEnd();
            _stateMachine.CurrentState = _stateMachine.Recovering;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //_enemy.StartCoroutine(ChargePlayer());
        }
    }
}
