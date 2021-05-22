using UnityEngine;
using System.Collections;
using hulaohyes.Assets.Scripts.Timers;

namespace hulaohyes.Assets.Scripts.Components.Enemy.Walker
{
    public class WalkerRush : EnemyComponent
    {
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
            enemy.rb.isKinematic = false;
            currentTimer = new Timer(enemy.enemyDataSet.activeDuration);
            currentTimer.onTimerEnd = EndRush;
            currentTimer.onTick = RushTarget;
        }

        void EndRush()
        {
            currentTimer = new Timer(enemy.enemyDataSet.recoveryDuration);
            currentTimer.onTimerEnd = Recover;
        }

        void Recover()
        {
            enemy.onTargetAquire += StartUpRush;

            currentTimer.Disable();
            currentTimer = null;
            enemy.rb.isKinematic = true;
        }

        void UpdateDirection()
        {
            if (target != null)
                enemy.direction = transform.position.x > target.position.x ? -1 : 1;
        }

        void RushTarget()
        {
            Vector2 lVelocity = new Vector2(enemy.direction * enemy.enemyDataSet.rushSpeed, enemy.rb.velocity.y);
            enemy.rb.velocity = lVelocity;
        }
    }
}