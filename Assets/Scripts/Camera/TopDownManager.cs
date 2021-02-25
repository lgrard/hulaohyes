using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.camera.elements;
using hulaohyes.player;
using Cinemachine;

namespace hulaohyes.camera
{
    public class TopDownManager : MonoBehaviour
    {
        [Header("Zone")]
        [SerializeField] Transform _zone;
        [SerializeField] Transform _camPosition;
        [SerializeField] bool _drawGizmos;

        [Header("Camera 0")]
        [SerializeField] CinemachineVirtualCamera _tdCam0;
        [SerializeField] CinemachineVirtualCamera _tdGlobalCam0;

        [Header("Camera 1")]
        [SerializeField] CinemachineVirtualCamera _tdCam1;
        [SerializeField] CinemachineVirtualCamera _tdGlobalCam1;

        private CameraManager _camManager;
        private TopDownCameraElement _tdCamElement0;
        private TopDownCameraElement _tdCamElement1;
        private BoxCollider _triggerZone;
        private bool _player0in = false;
        private bool _player1in = false;

        private void Start()
        {
            _camManager = CameraManager.getInstance();
            Camera lCam0 = _camManager.GetCamera(0);
            Camera lCam1 = _camManager.GetCamera(1);
            Transform lPlayerGroup = _camManager.PlayerGroup;
            Transform lPlayer0 = GameManager.getPlayer(0).transform;
            Transform lPlayer1 = GameManager.getPlayer(1).transform;

            _tdCamElement0 = new TopDownCameraElement(lCam0, _tdCam0, _tdGlobalCam0, lPlayer0, lPlayerGroup, _camPosition);
            _tdCamElement1 = new TopDownCameraElement(lCam1, _tdCam1, _tdGlobalCam1, lPlayer1, lPlayerGroup, _camPosition);

            CreateTriggerZone();
        }

        void CreateTriggerZone()
        {
            _triggerZone = gameObject.AddComponent<BoxCollider>();
            _triggerZone.isTrigger = true;
            _triggerZone.size = _zone.localScale;
            _triggerZone.center = _zone.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                int lPlayerIndex = lPlayer.playerIndex;

                if (lPlayerIndex == 0) _player0in = true;
                else if (lPlayerIndex == 1) _player1in = true;

                 TopDownCameraElement lTopDownCameraElement = lPlayerIndex == 0 ? _tdCamElement0 : _tdCamElement1;
                _camManager.SetCameraElement(lPlayerIndex, lTopDownCameraElement);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                int lPlayerIndex = lPlayer.playerIndex;

                if (lPlayerIndex == 0) _player0in = false;
                else if (lPlayerIndex == 1) _player1in = false;

                if(!_player0in && _player1in) _camManager.ResetCameraElement(lPlayerIndex);
            }
        }

        private void OnDrawGizmos()
        {
            if (_zone != null && _drawGizmos)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(_zone.localPosition, _zone.localScale);
            }

            if (_camPosition != null && _drawGizmos)
            {
                Gizmos.matrix = Matrix4x4.TRS(_camPosition.position, _camPosition.rotation, transform.lossyScale);
                Gizmos.color = Color.cyan;
                Gizmos.DrawFrustum(_camPosition.localPosition, 60, 1, 5000, 1.7f);
            }
        }
    }
}
