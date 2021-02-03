using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.enemy
{
    public class Enemy
    {
        protected int HP = 1;
        protected float speed = 1f;

        ///Create a new enemy object
        /// <param name="pHP">Amount of enemy life points</param>
        /// <param name="pSpeed">Enemy movement speed</param>
        public Enemy(int pHP,float pSpeed)
        {
            HP = pHP;
            speed = pSpeed;
        }

        /// The enemy takes damage and check the amount of HPs
        /// <param name="damage">Amount of damage taken by the enemy</param>
        public void takeDamage(int damage)
        {
            if (HP - damage <= 0)
            {
                //dies
            }

            else
                HP -= damage;
        }

        ///Destroy enemy's object and delete references
        public Enemy destroyEnemy()
        {
            //Destructor
            return this;
        }
    }
}
