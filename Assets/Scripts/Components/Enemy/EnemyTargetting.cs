using UnityEngine;
using hulaohyes.Assets.Scripts.Collision;

namespace hulaohyes.Assets.Scripts.Components.Enemy
{
    public class EnemyTargetting : EnemyComponent
    {
        private delegate Transform TriggerEvent(Collider2D pCollision);
        private TriggerEvent onTriggerEvent;

        [SerializeField] private string targetTag = "Player";
        [SerializeField] private Transform target;
        private CircleCollider2D detectionZone;

        private void OnEnable()
        {
            FrameTargetCheck();
            onTriggerEvent += TargetCheck;
        }
        private void OnDisable()
        {
            target = null;
            onTriggerEvent -= TargetCheck;
        }

        void Start() => Init();

        private void Init()
        {
            detectionZone = gameObject.AddComponent<CircleCollider2D>();
            detectionZone.isTrigger = true;
            detectionZone.radius = enemy.enemyDataSet.detectionRadius;
        }

        private void FrameTargetCheck()
        {
            Collider2D[] lColliders = Physics2D.OverlapCircleAll(transform.position, enemy.enemyDataSet.detectionRadius);
            foreach (Collider2D lCollider in lColliders)
                if (TargetCheck(lCollider) != null) break;
        }

        private Transform TargetCheck(Collider2D pCollision)
        {
            if (!pCollision.isTrigger && pCollision.CompareTag(targetTag))
            {
                target = pCollision.transform;
                enemy.onTargetAquire?.Invoke(target);
                return target;
            }
            return null;
        }

        private void OnTriggerEnter2D(Collider2D collision) => onTriggerEvent?.Invoke(collision);
    }
}