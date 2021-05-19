using hulaohyes.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.camera.elements;
using Cinemachine;

namespace hulaohyes.camera
{
    public class AngledCameraManager : AltCameraManager
    {
        private const float DEG2RAD = Mathf.PI/ 180;
        private const float RAD2DEG = 180 / Mathf.PI;

        [Header("Values")]
        [Range(-10,10)][SerializeField] float screenOffset = 0;
        [Range(0, 359)][SerializeField] float targetAngle = 0;
        [Range(1, 50)][SerializeField] float distance = 10;
        [SerializeField] bool isDollyToFit;

        [Header("Debug Preview")]
        [SerializeField] Vector3 playerDebugPos;
        [SerializeField] Transform camPosition;

        protected override void Init()
        {
            base.Init();
            Camera lCam0 = camManager.GetCamera(0);
            Transform lPlayerGroup = camManager.PlayerGroup;
            Transform lPlayer0 = GameManager.getPlayer(0).transform;
            float lScreenOffsetCM = (screenOffset/17.5f) + 0.5f;

            altCamElement = new AngledCameraElement(lCam0, altGlobalCam,lPlayerGroup, lPlayer0, lScreenOffsetCM, targetAngle,distance,isDollyToFit);
        }

        protected override void GizmosDebug()
        {
            base.GizmosDebug();

            if (camPosition != null)
            {
                float lNDistance = -distance;
                float lOffsetLength = Mathf.Sqrt(Mathf.Pow(lNDistance, 2) + Mathf.Pow(screenOffset, 2));
                float lOffsetAngle = -targetAngle * DEG2RAD + Mathf.Atan2(screenOffset, lNDistance);
                Vector3 lCamPosOffset = new Vector3(0, lOffsetLength * Mathf.Sin(lOffsetAngle), lOffsetLength * Mathf.Cos(lOffsetAngle));
                lCamPosOffset += playerDebugPos;

                Vector3 lCamAngle = new Vector3(targetAngle, transform.eulerAngles.y, 0);

                Gizmos.color = Color.cyan;
                Gizmos.matrix = Matrix4x4.TRS(camPosition.position, camPosition.rotation, transform.lossyScale);
                Gizmos.DrawFrustum(Vector3.zero, 60, 1, 5000, 1.7f);

                Gizmos.color = Color.black;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Vector3 lCamPosNormal = new Vector3(0, lNDistance * Mathf.Sin(-targetAngle * DEG2RAD), lNDistance * Mathf.Cos(-targetAngle * DEG2RAD));
                lCamPosNormal += playerDebugPos;
                Gizmos.DrawLine(lCamPosNormal, playerDebugPos);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(lCamPosNormal, lCamPosOffset);

                camPosition.localPosition = lCamPosOffset;
                camPosition.eulerAngles = lCamAngle;
            }

            Gizmos.DrawSphere(playerDebugPos, 0.5f);
        }
    }
}
