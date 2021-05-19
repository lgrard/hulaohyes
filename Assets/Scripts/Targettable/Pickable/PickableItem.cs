using System.Collections;
using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Player;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable
{
    public class PickableItem : MonoBehaviour, IPickable, ITargettable
    {
        private Rigidbody2D rb;
        private bool isPickable = true;

        private void Start()
        {
            if (TryGetComponent<Rigidbody2D>(out Rigidbody2D lRb))
                rb = lRb;

            else
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
            }
        }

        public void GetPickedUp(PlayerController2D pPicker)
        {
            rb.isKinematic = true;
            transform.eulerAngles = Vector3.zero;
            isPickable = false;
        }

        public void GetThrown(Vector2 pVelocity)
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.velocity = pVelocity;
            isPickable = true;
        }

        public void GetDropped(Vector2 pVelocity)
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.velocity = pVelocity;
            isPickable = true;
        }

        public bool isTargettable
        {
            get { return isPickable; }
        }

        public Transform Transform
        {
            get { return transform; }
        }
    }
}