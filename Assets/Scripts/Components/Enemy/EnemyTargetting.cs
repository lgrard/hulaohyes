using UnityEngine;

namespace hulaohyes.Assets.Scripts.Components.Enemy
{
    public class EnemyTargetting : EnemyComponent
    {
        [SerializeField] private string targetTag = "Player";
        [SerializeField] Transform target;
        private CircleCollider2D detectionZone;

        void Start() => Init();

        void Init()
        {
            detectionZone = gameObject.AddComponent<CircleCollider2D>();
            detectionZone.isTrigger = true;
            detectionZone.radius = enemy.enemyDataSet.detectionRadius;
        }

        void FrameTargetCheck()
        {
            Collider2D[] lColliders = Physics2D.OverlapCircleAll(transform.position, enemy.enemyDataSet.detectionRadius);
            foreach (Collider2D lCollider in lColliders) TargetCheck(lCollider);
        }

        void TargetCheck(Collider2D pCollision)
        {
            if (!pCollision.isTrigger && pCollision.CompareTag(targetTag))
            {
                target = pCollision.transform;
                enemy.onTargetAquire?.Invoke(target);
                return;
            }
            return;
        }

        private void OnTriggerEnter2D(Collider2D collision) => TargetCheck(collision);

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.transform == target)
            {
                target = null;
                enemy.onTargetLoss?.Invoke();
            }
        }
    }
}