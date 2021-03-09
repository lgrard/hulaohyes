using System.Collections;
using System.Collections.Generic;
using hulaohyes.player;
using UnityEngine;

namespace hulaohyes.enemy
{
    public class Projectile : MonoBehaviour
    {
        private const float PROJECTILE_SPEED = 5f;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            rb.velocity = transform.forward * PROJECTILE_SPEED;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController pPlayer))
                pPlayer.TakeDamage(1, transform);

            Destroy(gameObject);
        }
    }
}
