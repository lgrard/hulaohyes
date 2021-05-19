using hulaohyes.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy
{
    public class DummyController : Pickable
    {
        private Animator animator;

        [SerializeField] ParticleSystem deathParticles;

        protected override void Init()
        {
            base.Init();
            animator = GetComponent<Animator>();
        }

        public override void GetPicked(PlayerController pPicker)
        {
            base.GetPicked(pPicker);
            animator.SetBool("Carried", true);
        }

        public override void Propel()
        {
            animator.SetTrigger("Thrown");
            base.Propel();
        }

        public override void Drop()
        {
            animator.SetBool("Carried", false);
            animator.SetTrigger("Escape");
            base.Drop();
        }

        protected override void HitElseThrown(Collider pCollider)
        {
            destroyEnemy();
        }

        protected override void HitElseDropped(Collider pCollider)
        {
            animator.SetTrigger("HitGround");
            base.HitElseDropped(pCollider);
        }

        protected override void Drowns()
        {
            base.Drowns();
            destroyEnemy();
        }

        public void destroyEnemy()
        {
            deathParticles.Play();
            animator.SetTrigger("TakeDamage");
            _collider.isTrigger = true;
            rb.isKinematic = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
