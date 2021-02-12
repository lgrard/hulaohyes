using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy
{
    public class EnemyController : Pickable
    {
        protected int HP = 1;
        protected float speed = 1f;

        /// The enemy takes damage and check the amount of HPs
        /// <param name="damage">Amount of damage taken by the enemy</param>
        public void takeDamage(int damage)
        {
            if (HP - damage <= 0)
            {
                //dies
                Debug.Log(gameObject.name+" is dead");
            }

            else
                HP -= damage;
        }

        public void destroyEnemy()
        {
            //Destructor
        }
    }
}
