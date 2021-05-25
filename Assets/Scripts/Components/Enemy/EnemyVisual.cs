using UnityEngine;
using System.Collections;

namespace hulaohyes.Assets.Scripts.Components.Enemy
{
    public class EnemyVisual : EnemyComponent
    {
        [SerializeField] private Animator animator = null;

        [Header("Particles")]
        [SerializeField] private ParticleSystem startUpParticles = null;
        [SerializeField] private ParticleSystem activeParticles = null;
        [SerializeField] private ParticleSystem endActiveParticles = null;
        [SerializeField] private ParticleSystem hitParticles = null;

        private int lastDirection = 0;

        private void OnEnable()
        {
            enemy.onTargetAquire += OnTargetAquire;
            enemy.onActive += OnActive;
            enemy.onRecover += OnRecover;
            enemy.onEndAttack += OnEndAttack;
            enemy.onChangeDirection += OnChangeDirection;
            enemy.onGetPickedUp += OnGetPickedUp;
            enemy.onGetDropped += OnGetDropped;
            enemy.onGetThrown += OnGetThrown;
        }
        
        private void OnDisable()
        {
            enemy.onTargetAquire -= OnTargetAquire;
            enemy.onActive -= OnActive;
            enemy.onRecover -= OnRecover;
            enemy.onEndAttack -= OnEndAttack;
            enemy.onChangeDirection -= OnChangeDirection;
            enemy.onGetPickedUp -= OnGetPickedUp;
            enemy.onGetDropped -= OnGetDropped;
            enemy.onGetThrown -= OnGetThrown;
        }

        void OnChangeDirection()
        {
            if (enemy.direction != lastDirection)
            {
                lastDirection = enemy.direction;

                float lDirection = lastDirection > 0 ? 90 : -90;
                animator.gameObject.transform.eulerAngles = new Vector3(0, lDirection, animator.gameObject.transform.eulerAngles.z);
            }
        }

        void OnTargetAquire(Transform pTarget)
        {
            enemy.onTargetAquire -= OnTargetAquire;
            animator.SetTrigger("SpotTarget");
            startUpParticles?.Play();
        }

        void OnActive()
        {
            animator.SetTrigger("Attacks");
            startUpParticles?.Stop();
            activeParticles?.Play();
        }

        void OnRecover()
        {
            animator.SetBool("isRecovering", true);
            activeParticles?.Stop();
            endActiveParticles?.Play();
        }

        void OnEndAttack()
        {
            enemy.onTargetAquire += OnTargetAquire;
            animator.SetBool("isRecovering", false);
        }

        void OnGetPickedUp()
        {
            animator.SetTrigger("GetPickedUp");
        }

        void OnGetDropped()
        {
            animator.SetTrigger("Escape");
        }
        void OnGetThrown()
        {
            animator.SetTrigger("Thrown");
        }
    }
}