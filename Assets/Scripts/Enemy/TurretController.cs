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
            stateMachine = new TurretStateMachine(this, rb, enemyAnimator, enemyParticles, detectionZone);
        }
    }
}
