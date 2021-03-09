using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace hulaohyes.enemy.states.turret
{
    public class Shooting : Attacking
    {
        private Vector3 offset = new Vector3(0,0.5f,0);
        private GameObject projectilePrefab;

        public Shooting(EnemyStateMachine pStateMachine, EnemyController pEnemy, Animator pAnimator, GameObject pProjectilePrefab)
            : base(pStateMachine, pEnemy, pAnimator)
        {
            projectilePrefab = pProjectilePrefab;
            MAX_TIMER = 1f;
        }

        void Shoot()
        {
            GameObject lBullet = MonoBehaviour.Instantiate(projectilePrefab);
            lBullet.transform.rotation = enemy.transform.rotation;
            lBullet.transform.position = enemy.transform.position + offset;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Shoot();
        }
    }
}
