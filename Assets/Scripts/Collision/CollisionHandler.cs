using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace hulaohyes.Assets.Scripts.Collision
{
    public class CollisionHandler : MonoBehaviour
    {
        public delegate void ObjectCollisionHandler(Collision2D pCollision);
        public event ObjectCollisionHandler objectCollisionEnter;
        public event ObjectCollisionHandler objectCollisionExit;

        private void OnCollisionEnter2D(Collision2D collision) { objectCollisionEnter?.Invoke(collision); }
        private void OnCollisionExit2D(Collision2D collision) { objectCollisionExit?.Invoke(collision); }
    }
}
