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


        ///Destroy enemy's object and delete references
        public Enemy destroyEnemy()
        {
            //Destructor
            return this;
        }
    }
}
