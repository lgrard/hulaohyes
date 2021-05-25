using UnityEngine;
using hulaohyes.Assets.Scripts.Timers;

namespace hulaohyes.Assets.Scripts.Components.Enemy
{
    public class EnemyWiggling : EnemyComponent
    {
        private Timer currentTimer = null;

        private void OnEnable()
        {
            enemy.onGetPickedUp += StartWiggle;
            enemy.onGetDropped += InterruptWiggle;
            enemy.onGetThrown += InterruptWiggle;
        }

        private void OnDisable()
        {
            enemy.onGetPickedUp -= StartWiggle;
            enemy.onGetDropped -= InterruptWiggle;
            enemy.onGetThrown -= InterruptWiggle;
            InterruptWiggle();
        }

        void StartWiggle()
        {
            currentTimer = new Timer(enemy.enemyDataSet.wiggleDuration);
            currentTimer.onTimerEnd += EndWiggle;
        }

        void InterruptWiggle()
        {
            currentTimer?.Disable();
            currentTimer = null;
        }

        void EndWiggle()
        {
            InterruptWiggle();
            Debug.Log("End wiggle");
        }
    }
}