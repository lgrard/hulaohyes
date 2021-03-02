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
        [SerializeField] protected CinemachineVirtualCamera _altGlobalCam;
        [SerializeField] protected CinemachineVirtualCamera _secondaryGlobalCam;

        [Header("Zone")]
        [SerializeField] Transform _zone;
        [SerializeField] bool _drawGizmos;

        private BoxCollider _triggerZone;
        protected CameraManager _camManager;
        protected bool _player0in = false;
        protected bool _player1in = false;

        [SerializeField] protected CameraElement _altCamElement;

        private void Start() => Init();

        protected virtual void Init()
        {
            _camManager = CameraManager.getInstance();
            gameObject.transform.localScale = Vector3.one;

            CreateTriggerZone();

            _altGlobalCam.gameObject.SetActive(true);
            _secondaryGlobalCam.gameObject.SetActive(true);
        }

        private void CreateTriggerZone()
        {
            _triggerZone = gameObject.AddComponent<BoxCollider>();
            _triggerZone.isTrigger = true;
            _triggerZone.size = _zone.localScale;
            _triggerZone.center = _zone.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                int lPlayerIndex = lPlayer.playerIndex;
                if (lPlayerIndex == 0) _player0in = true;
                else if (lPlayerIndex == 1) _player1in = true;

                if(_player0in && _player1in) ZoneEnter(lPlayer);
            }
        }

        private void ZoneEnter(PlayerController pPlayer)
        {
            _secondaryGlobalCam.m_Priority = 120;
            _camManager.SetAltCameraElement(_altCamElement);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                if (_player0in && _player1in) ZoneExit(lPlayer);

                int lPlayerIndex = lPlayer.playerIndex;
                if (lPlayerIndex == 0) _player0in = false;
                else if (lPlayerIndex == 1) _player1in = false;
            }
        }

        private void ZoneExit(PlayerController pPlayer)
        {
            _secondaryGlobalCam.m_Priority = -1;
            _camManager.ResetCamerasElement();
        }

        protected virtual void GizmosDebug()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(transform.position, 0.25f);

            if (_zone != null)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(_zone.localPosition, _zone.localScale);
            }
        }

        private void OnDrawGizmos() { if (_drawGizmos) GizmosDebug(); }
    }
}
