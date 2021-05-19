using UnityEngine;

namespace hulaohyes.Assets.Scripts.Components.Gravity
{
    public class ObjectGravity : MonoBehaviour
    {
        private Rigidbody2D rb;
        [SerializeField] private GravityDatas gravityData;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            if (!rb.isKinematic)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - gravity);

            else rb.velocity = Vector2.zero;
        }

        private float gravity => rb.velocity.y > 0 ? gravityData.risingGravity:gravityData.fallingGravity;
    }
}