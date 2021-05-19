using System.Collections;
using UnityEngine;

namespace hulaohyes.Assets.Scripts.Timers
{
    public class TimerHandler : MonoBehaviour
    {
        private static TimerHandler instance;
        public static System.Action onUpdate;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(gameObject);
        }

        void Update() => onUpdate?.Invoke();

        /// Returns the timer handler instance
        /// <returns> Returns timer handler unique instance </returns>
        public static TimerHandler getInstance()
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<TimerHandler>();
                instance.gameObject.name = "TimerHandler";
            }

            return instance;
        }
    }
}