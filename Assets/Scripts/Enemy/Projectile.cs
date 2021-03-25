using System.Collections;
using System.Collections.Generic;
using hulaohyes.player;
using UnityEngine;

namespace hulaohyes.enemy
{
    public class Projectile : MonoBehaviour
    {
        public float PROJECTILE_LIFETIME = 3f;          //const to change
        public float PROJECTILE_SPEED = 5f;       //const to change
        private float destroyTimer;
        private ParticleSystem hitParticles;

        private void Start()
        {
            destroyTimer = PROJECTILE_LIFETIME;
            hitParticles = GetComponentInChildren<ParticleSystem>();
        }

        private void FixedUpdate()
        {
            transform.Translate(transform.forward * PROJECTILE_SPEED*Time.deltaTime,Space.World);

            if (destroyTimer > 0) destroyTimer -= Time.deltaTime;
            else Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<TurretController>(out TurretController lTurret) || other.isTrigger) return;

            if(other.TryGetComponent<PlayerController>(out PlayerController pPlayer))
                pPlayer.TakeDamage(1, transform);
            hitParticles.transform.parent = null;
            hitParticles.transform.position = transform.position;
            hitParticles.Play();
            Destroy(hitParticles, 0.5f);
            Destroy(gameObject);
        }
    }
}
