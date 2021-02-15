using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;

namespace hulaohyes.enemy
{
    public class EnemyController : Pickable
    {
        protected int maxHP = 10;
        protected int _hp = 1;
        protected float speed = 1f;

        private void Start() => Init();

        /// The enemy takes damage and check the amount of HPs
        /// <param name="damage">Amount of damage taken by the enemy</param>
        public void takeDamage(int damage)
        {
            if (_hp - damage <= 0)
            {
                //dies
                Debug.Log(gameObject.name+" is dead");
            }

            else
                _hp -= damage;
        }

        protected override void Init()
        {
            base.Init();
            _hp = maxHP;
        }

        public void destroyEnemy()
        {
            //Destructor
        }
    }
}
