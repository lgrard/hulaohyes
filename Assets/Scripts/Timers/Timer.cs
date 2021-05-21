using System.Collections;
using UnityEngine;

namespace hulaohyes.Assets.Scripts.Timers
{
    public class Timer
    {
        public delegate void TimerAction();
        public TimerAction onTimerEnd;

        private float timer;

        /// Custom timer class
        /// <param name="pTime"> Timer duration </param>
        public Timer(float pTime)
        {
            timer = pTime;
            TimerHandler.getInstance();
            TimerHandler.onUpdate += TimerTick;
        }

        void TimerTick()
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                onTimerEnd?.Invoke();
                TimerHandler.onUpdate -= TimerTick;
            }
        }
    }
}