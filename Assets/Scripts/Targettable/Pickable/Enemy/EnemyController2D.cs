using System.Collections;
using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Player;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable.Enemy
{
    public class EnemyController2D : MonoBehaviour, IPickable, ITargettable
    {
        public delegate void EventHandler();
        public EventHandler onGetThrown;
        public EventHandler onGetDropped;
        public EventHandler onGetPickedUp;

        private Rigidbody2D rb;
        private bool isPickable = true;

        private void Start() => Init();

        private void Init()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void GetThrown(Vector2 pVelocity)
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.velocity = pVelocity;
            isPickable = true;
            onGetThrown?.Invoke();
        }
        
        public void GetDropped(Vector2 pVelocity)
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.velocity = pVelocity;
            isPickable = true;
            onGetDropped?.Invoke();
        }

        public void GetPickedUp(PlayerController2D pPicker)
        {
            rb.isKinematic = true;
            transform.eulerAngles = Vector3.zero;
            isPickable = false;
            onGetPickedUp?.Invoke();
        }

        public bool isTargettable => isPickable;
        public Transform Transform => transform;
    }
}