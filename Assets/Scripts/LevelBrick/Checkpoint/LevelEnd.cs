using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;

namespace hulaohyes.levelbrick.checkpoint
{
    public class LevelEnd : MonoBehaviour
    {
        private bool player0in = false;
        private bool player1in = false;

        [Header("Values")]
        [SerializeField] private Vector3 triggerZoneSize;

        [SerializeField] bool drawGizmos = true;

        private void Start()
        {
            BoxCollider lTrigger = gameObject.AddComponent<BoxCollider>();
            lTrigger.isTrigger = true;
            lTrigger.size = triggerZoneSize;
            lTrigger.center = new Vector3(0, lTrigger.size.y/2, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                if (lPlayer.playerIndex == 0) player0in = true;
                else player1in = true;
            }

            if (player0in && player1in) Debug.Log("Win !");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                if (lPlayer.playerIndex == 0) player0in = false;
                else player1in = false;
            }

        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                Gizmos.color = Color.blue;
                Gizmos.matrix = Matrix4x4.TRS(transform.position,transform.rotation,transform.lossyScale);
                Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, triggerZoneSize.y/2, 0), triggerZoneSize);
            }
        }
    }
}
