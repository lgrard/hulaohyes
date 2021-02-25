using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class TopDownCameraElement : CameraElement
    {
        private CinemachineComposer _tdCamTransposer;

        public TopDownCameraElement(Camera pCamera, CinemachineVirtualCamera pTDCam,
            CinemachineVirtualCamera pTDCamGlobal, Transform pTarget, Transform pPlayerGroup, Transform pPosition)
            : base(pCamera, pTarget)
        {
            _camNormal = pTDCam;
            _camGlobal = pTDCamGlobal;
            _tdCamTransposer = _camNormal.GetCinemachineComponent<CinemachineComposer>();

            _camNormal.Follow = pPosition;
            _camGlobal.Follow = pPosition;
            _camNormal.LookAt = _target;
            _camGlobal.LookAt = pPlayerGroup;
        }

        public CinemachineComposer TDCamTransposer
        {
            get => _tdCamTransposer;
        }
    }
}