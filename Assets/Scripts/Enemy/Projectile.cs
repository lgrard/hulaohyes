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
        private Rigidbody rb;
        private float destroyTimer;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            destroyTimer = PROJECTILE_LIFETIME;
        }

        private void FixedUpdate()
        {
            rb.velocity = transform.forward * PROJECTILE_SPEED;

            if (destroyTimer > 0) destroyTimer -= Time.deltaTime;
            else Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController pPlayer))
                pPlayer.TakeDamage(1, transform);

            Destroy(gameObject);
        }
    }
}
