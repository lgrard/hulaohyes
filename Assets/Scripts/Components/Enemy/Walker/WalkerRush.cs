﻿using UnityEngine;
using System.Collections;
using hulaohyes.Assets.Scripts.Timers;

namespace hulaohyes.Assets.Scripts.Components.Enemy.Walker
{
    public class WalkerRush : EnemyComponent
    {
        Collider2D attackZone;
        float attackZoneOffset;
        Timer currentTimer;
        Transform target;

        private void OnEnable()
        {
            enemy.onTargetAquire += StartUpRush;
        }

        private void OnDisable()
        {
            enemy.onTargetAquire -= StartUpRush;
            currentTimer?.Disable();
            currentTimer = null;
        }

        private void Start()
        {
            attackZone = GetComponent<Collider2D>();
            attackZoneOffset = Mathf.Abs(attackZone.offset.x);
        }

        void StartUpRush(Transform pTarget)
        {
            enemy.onTargetAquire -= StartUpRush;

            target = pTarget;
            enemy.rb.isKinematic = true;
            enemy.rb.velocity = Vector2.zero;
            currentTimer = new Timer(enemy.enemyDataSet.startUpDuration);
            currentTimer.onTimerEnd = ActiveRush;
            currentTimer.onTick = UpdateDirection;
        }

        void ActiveRush()
        {
            enemy.onActive?.Invoke();
            enemy.rb.isKinematic = false;
            currentTimer = new Timer(enemy.enemyDataSet.activeDuration);
            currentTimer.onTimerEnd = EndRush;
            currentTimer.onTick = RushTarget;
        }

        void EndRush()
        {
            enemy.onRecover?.Invoke();
            enemy.rb.isKinematic = true;
            currentTimer = new Timer(enemy.enemyDataSet.recoveryDuration);
            currentTimer.onTimerEnd = Recover;
        }

        void Recover()
        {
            enemy.onTargetAquire += StartUpRush;
            currentTimer.Disable();
            currentTimer = null;

            enemy.onEndAttack?.Invoke();
        }

        void UpdateDirection()
        {
            if (target != null)
            {
                enemy.direction = transform.position.x > target.position.x ? -1 : 1;
                attackZone.offset = new Vector2 (attackZoneOffset * enemy.direction, attackZone.offset.y);
                enemy.onChangeDirection?.Invoke();
            }
        }

        void RushTarget()
        {
            Vector2 lVelocity = new Vector2(enemy.direction * enemy.enemyDataSet.rushSpeed, enemy.rb.velocity.y);
            enemy.rb.velocity = lVelocity;
        }
    }
}