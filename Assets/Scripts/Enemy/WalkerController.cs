﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states.walker;
using hulaohyes.player;
using UnityEngine.AI;

namespace hulaohyes.enemy
{
    public class WalkerController : EnemyController
    {
        private NavMeshAgent navMeshAgent;
        private BoxCollider damageZone;
        [SerializeField] Transform damageZoneSetting;


        protected override void Init()
        {
            base.Init();
            CreateDamageZone();
            navMeshAgent = GetComponent<NavMeshAgent>();
            stateMachine = new WalkerStateMachine(this, rb, enemyAnimator, enemyParticles, navMeshAgent, detectionZone, damageZone);
            isPickableState = false;
        }

        private void CreateDamageZone()
        {
            damageZone = gameObject.AddComponent<BoxCollider>();
            damageZone.size = damageZoneSetting.localScale;
            damageZone.center = damageZoneSetting.localPosition;
            damageZone.isTrigger = true;
            damageZone.enabled = false;
        }

        public override void GetPicked(PlayerController pPlayer)
        {
            navMeshAgent.enabled = false;
            base.GetPicked(pPlayer);
        }

        protected override void HitElseDropped(Collider pCollider)
        {
            navMeshAgent.enabled = true;
            base.HitElseDropped(pCollider);
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
