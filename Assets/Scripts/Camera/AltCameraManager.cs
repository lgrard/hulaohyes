using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.camera.elements;
using Cinemachine;

namespace hulaohyes.camera
{
    public abstract class AltCameraManager : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] protected CinemachineVirtualCamera altGlobalCam;
        [SerializeField] protected CinemachineVirtualCamera secondaryGlobalCam;

        [Header("Zone")]
        [SerializeField] Transform zone;
        [SerializeField] bool drawGizmos;

        private BoxCollider triggerZone;
        protected CameraManager camManager;
        protected bool player0in = false;
        protected bool player1in = false;

        [SerializeField] protected CameraElement altCamElement;

        private void Start() => Init();

        protected virtual void Init()
        {
            camManager = CameraManager.getInstance();
            gameObject.transform.localScale = Vector3.one;

            CreateTriggerZone();

            altGlobalCam.gameObject.SetActive(true);
            secondaryGlobalCam.gameObject.SetActive(true);
        }

        private void CreateTriggerZone()
        {
            triggerZone = gameObject.AddComponent<BoxCollider>();
            triggerZone.isTrigger = true;
            triggerZone.size = zone.localScale;
            triggerZone.center = zone.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                int lPlayerIndex = lPlayer.playerIndex;
                if (lPlayerIndex == 0) player0in = true;
                else if (lPlayerIndex == 1) player1in = true;

                if(player0in && player1in) ZoneEnter(lPlayer);
            }
        }

        private void ZoneEnter(PlayerController pPlayer)
        {
            secondaryGlobalCam.m_Priority = 120;
            camManager.SetAltCameraElement(altCamElement);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                if (player0in && player1in) ZoneExit(lPlayer);

                int lPlayerIndex = lPlayer.playerIndex;
                if (lPlayerIndex == 0) player0in = false;
                else if (lPlayerIndex == 1) player1in = false;
            }
        }

        private void ZoneExit(PlayerController pPlayer)
        {
            secondaryGlobalCam.m_Priority = -1;
            camManager.ResetCamerasElement();
        }

        protected virtual void GizmosDebug()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(transform.position, 0.25f);

            if (zone != null)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(zone.localPosition, zone.localScale);
            }
        }

        private void OnDrawGizmos() { if (drawGizmos) GizmosDebug(); }
    }
}
