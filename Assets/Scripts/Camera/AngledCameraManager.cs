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
        [Range(-10,10)][SerializeField] float _screenOffset = 0;
        [Range(0, 359)][SerializeField] float _targetAngle = 0;
        [Range(1, 50)][SerializeField] float _distance = 10;
        [SerializeField] bool _isDollyToFit;

        [Header("Debug Preview")]
        [SerializeField] Vector3 _playerDebugPos;
        [SerializeField] Transform _camPosition;

        protected override void Init()
        {
            base.Init();
            Camera lCam0 = _camManager.GetCamera(0);
            Transform lPlayerGroup = _camManager.PlayerGroup;
            Transform lPlayer0 = GameManager.getPlayer(0).transform;
            float lScreenOffsetCM = (_screenOffset/17.5f) + 0.5f;

            _altCamElement = new AngledCameraElement(lCam0, _altGlobalCam,lPlayerGroup, lPlayer0, lScreenOffsetCM, _targetAngle,_distance,_isDollyToFit);
        }

        protected override void GizmosDebug()
        {
            base.GizmosDebug();

            if (_camPosition != null)
            {
                float lNDistance = -_distance;
                float lOffsetLength = Mathf.Sqrt(Mathf.Pow(lNDistance, 2) + Mathf.Pow(_screenOffset, 2));
                float lOffsetAngle = -_targetAngle * DEG2RAD + Mathf.Atan2(_screenOffset, lNDistance);
                Vector3 lCamPosOffset = new Vector3(0, lOffsetLength * Mathf.Sin(lOffsetAngle), lOffsetLength * Mathf.Cos(lOffsetAngle));
                lCamPosOffset += _playerDebugPos;

                Vector3 lCamAngle = new Vector3(_targetAngle, transform.eulerAngles.y, 0);

                Gizmos.color = Color.cyan;
                Gizmos.matrix = Matrix4x4.TRS(_camPosition.position, _camPosition.rotation, transform.lossyScale);
                Gizmos.DrawFrustum(Vector3.zero, 60, 1, 5000, 1.7f);

                Gizmos.color = Color.black;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Vector3 lCamPosNormal = new Vector3(0, lNDistance * Mathf.Sin(-_targetAngle * DEG2RAD), lNDistance * Mathf.Cos(-_targetAngle * DEG2RAD));
                lCamPosNormal += _playerDebugPos;
                Gizmos.DrawLine(lCamPosNormal, _playerDebugPos);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(lCamPosNormal, lCamPosOffset);

                _camPosition.localPosition = lCamPosOffset;
                _camPosition.eulerAngles = lCamAngle;
            }

            Gizmos.DrawSphere(_playerDebugPos, 0.5f);
        }
    }
}
