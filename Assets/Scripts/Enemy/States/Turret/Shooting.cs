using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.turret
{
    public class Shooting : Attacking
    {
        public Shooting(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            MAX_TIMER = 1f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Shoot");
        }
    }
}
