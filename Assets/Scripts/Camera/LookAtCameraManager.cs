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
        [SerializeField] Transform camPosition;

        protected override void Init()
        {
            base.Init();
            Camera lCam0 = camManager.GetCamera(0);
            Transform lPlayerGroup = camManager.PlayerGroup;
            Transform lPlayer0 = GameManager.getPlayer(0).transform;

            altCamElement = new LookAtCameraElement(lCam0, altGlobalCam, lPlayer0, lPlayerGroup, camPosition);
        }

        protected override void GizmosDebug()
        {
            base.GizmosDebug();

            if (camPosition != null)
            {
                Gizmos.matrix = Matrix4x4.TRS(camPosition.position, camPosition.rotation, transform.lossyScale);
                Gizmos.color = Color.cyan;
                Gizmos.DrawFrustum(camPosition.localPosition, 60, 1, 5000, 1.7f);
            }
        }
    }
}
