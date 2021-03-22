using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states.turret;

namespace hulaohyes.enemy
{
    public class TurretController : EnemyController
    {
        private Vector3 shootOffset = new Vector3(8, 0, 0);

        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform rig;

        protected override void Init()
        {
            base.Init();
            stateMachine = new TurretStateMachine(this, rb, enemyAnimator, enemyParticles, detectionZone);
        }

        private void LateUpdate()
        {
            rig.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, 0);
        }

        public void Shoot()
        {
            GameObject lBullet = MonoBehaviour.Instantiate(bulletPrefab);
            Projectile lBulletComp = lBullet.GetComponent<Projectile>();            //const to change
            lBulletComp.PROJECTILE_LIFETIME = PROJECTILE_LIFETIME;                  //const to change
            lBulletComp.PROJECTILE_SPEED = PROJECTILE_SPEED;                        //const to change
            lBullet.transform.rotation = transform.rotation;
            lBullet.transform.Rotate(shootOffset, Space.World);
            lBullet.transform.position = enemyParticles[2].transform.position;
        }
    }
}
