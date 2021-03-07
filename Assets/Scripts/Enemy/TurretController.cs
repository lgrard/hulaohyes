using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states.turret;

namespace hulaohyes.enemy
{
    public class TurretController : EnemyController
    {
        protected override void Init()
        {
            base.Init();
            _stateMachine = new TurretStateMachine(this, _rb, _enemyAnimator, _enemyParticles, _navMeshAgent, _detectionZone);
        }
    }
}
