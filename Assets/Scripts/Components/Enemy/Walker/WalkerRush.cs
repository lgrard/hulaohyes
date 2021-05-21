using UnityEngine;
using System.Collections;
using hulaohyes.Assets.Scripts.Timers;

namespace hulaohyes.Assets.Scripts.Components.Enemy.Walker
{
    public class WalkerRush : EnemyComponent
    {
        bool isActive = false;
        bool isStarting = false;
        bool isRecovering = false;
        Timer currentTimer;
        Transform target;

        private void OnEnable()
        {
            enemy.onTargetAquire += StartUpRush;
        }

        private void OnDisable()
        {
            enemy.onTargetAquire -= StartUpRush;
        }

        void StartUpRush(Transform pTarget)
        {
            isStarting = true;
            target = pTarget;
            enemy.rb.isKinematic = true;
            enemy.rb.velocity = Vector2.zero;
            currentTimer = new Timer(enemy.enemyDataSet.startUpDuration);
            currentTimer.onTimerEnd = ActiveRush;
        }

        void ActiveRush()
        {
            isStarting = false;
            isActive = true;
            enemy.rb.isKinematic = false;
            currentTimer = new Timer(enemy.enemyDataSet.activeDuration);
            currentTimer.onTimerEnd = EndRush;
        }

        void EndRush()
        {
            isActive = false;
            isRecovering = true;
            currentTimer = new Timer(enemy.enemyDataSet.recoveryDuration);
            currentTimer.onTimerEnd = Recover;
        }

        void Recover()
        {
            isRecovering = false;
            enemy.rb.isKinematic = true;
        }

        private void FixedUpdate()
        {
            if (isStarting && target != null)
            {
                float lDirection = 1;
            }

            if (isActive)
            {
                Vector2 lVelocity = new Vector2(5,enemy.rb.velocity.y);
                enemy.rb.velocity = lVelocity;
            }
        }
    }
}