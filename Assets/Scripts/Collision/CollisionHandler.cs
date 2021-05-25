using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace hulaohyes.Assets.Scripts.Collision
{
    public class CollisionHandler : MonoBehaviour
    {
        public delegate void ObjectCollisionHandler<T>(T pValue);
        public event ObjectCollisionHandler<Collision2D> objectCollisionEnter;
        public event ObjectCollisionHandler<Collision2D> objectCollisionExit;
        public event ObjectCollisionHandler<Collider2D> objectTriggerEnter;
        public event ObjectCollisionHandler<Collider2D> objectTriggerExit;

        private void OnCollisionEnter2D(Collision2D collision) { objectCollisionEnter?.Invoke(collision); }
        private void OnCollisionExit2D(Collision2D collision) { objectCollisionExit?.Invoke(collision); }
        private void OnTriggerEnter2D(Collider2D collider) { objectTriggerEnter?.Invoke(collider); }
        private void OnTriggerExit2D(Collider2D collider) { objectTriggerExit?.Invoke(collider); }
    }
}
