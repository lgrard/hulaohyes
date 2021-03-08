using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states.walker;
using hulaohyes.player;

namespace hulaohyes.enemy
{
    public class WalkerController : EnemyController
    {
        private BoxCollider damageZone;
        [SerializeField] Transform damageZoneSetting;

        protected override void Init()
        {
            base.Init();
            CreateDamageZone();
            stateMachine = new WalkerStateMachine(this, rb, enemyAnimator, enemyParticles, navMeshAgent, detectionZone, damageZone);
        }

        private void CreateDamageZone()
        {
            damageZone = gameObject.AddComponent<BoxCollider>();
            damageZone.size = damageZoneSetting.localScale;
            damageZone.center = damageZoneSetting.localPosition;
            damageZone.isTrigger = true;
            damageZone.enabled = false;
        }

        protected override void HitPlayer(PlayerController pPlayer)
        {
            if (isAttacking)
            {
                pPlayer.TakeDamage(1, transform);
                stateMachine.CurrentState = stateMachine.Recovering;
            }
            
            base.HitPlayer(pPlayer);
        }

        protected override void OnGizmos()
        {
            base.OnGizmos();
            Gizmos.color = Color.red;
            if (damageZoneSetting != null) Gizmos.DrawWireCube(damageZoneSetting.localPosition, damageZoneSetting.localScale);
        }
    }
}
