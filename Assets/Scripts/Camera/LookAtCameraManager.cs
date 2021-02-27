using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.camera.elements;
using hulaohyes.player;
using Cinemachine;

namespace hulaohyes.camera
{
    public class LookAtCameraManager : AltCameraManager
    {
        [Header("Camera")]
        [SerializeField] Transform _camPosition;

        protected override void Init()
        {
            base.Init();
            Camera lCam0 = _camManager.GetCamera(0);
            Transform lPlayerGroup = _camManager.PlayerGroup;
            Transform lPlayer0 = GameManager.getPlayer(0).transform;

            _altCamElement = new LookAtCameraElement(lCam0, _altGlobalCam, lPlayer0, lPlayerGroup, _camPosition);
        }

        protected override void GizmosDebug()
        {
            base.GizmosDebug();

            if (_camPosition != null)
            {
                Gizmos.matrix = Matrix4x4.TRS(_camPosition.position, _camPosition.rotation, transform.lossyScale);
                Gizmos.color = Color.cyan;
                Gizmos.DrawFrustum(_camPosition.localPosition, 60, 1, 5000, 1.7f);
            }
        }
    }
}
