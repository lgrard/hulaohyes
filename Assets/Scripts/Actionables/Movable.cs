using UnityEngine;
using hulaohyes.Assets.Scripts.Timers;

namespace hulaohyes.Assets.Scripts.Actionables
{
    public class Movable : Actionable
    {
        [SerializeField] private Transform objectToMove = null;
        [SerializeField] private Vector2 activePosition = Vector2.zero;
        [SerializeField] private bool parentPlayer = false;

        [Header("Time values")]
        [SerializeField] private float timeToMove = 1;
        [SerializeField] private AnimationCurve movingCurve = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
        private Vector2 basePosition = Vector2.zero;
        private Timer currentTimer = null;

        private void Start()
        {
            basePosition = objectToMove.localPosition;
        }

        public override void DoAction()
        {
            currentTimer = new Timer(timeToMove);
            currentTimer.onTick = ActionOn;
        }

        void ActionOn()
        {
            float lProgress = 1 - (currentTimer.currentTime / timeToMove);
            MoveObject(lProgress);
        }

        void ActionOff()
        {
            float lProgress = currentTimer.currentTime / timeToMove;
            MoveObject(lProgress);
        }

        void MoveObject(float pProgress) => objectToMove.localPosition = Vector2.Lerp(basePosition, activePosition, movingCurve.Evaluate(pProgress));
    }
}