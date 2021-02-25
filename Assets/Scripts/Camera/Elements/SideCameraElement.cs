using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class SideCameraElement : CameraElement
    {
        private CinemachineFramingTransposer _sideCamTransposer;


        public SideCameraElement(Camera pCamera, CinemachineVirtualCamera pSideCam, CinemachineVirtualCamera pSideCamGlobal, Transform pTarget, Transform pPlayerGroup)
            :base(pCamera,pTarget)
        {
            _camNormal = pSideCam;
            _camGlobal = pSideCamGlobal;
            _sideCamTransposer = _camNormal.GetCinemachineComponent<CinemachineFramingTransposer>();

            _camNormal.Follow = _target;
            _camGlobal.Follow = pPlayerGroup;
        }

        public CinemachineFramingTransposer SideCamTransposer
        {
            get => _sideCamTransposer;
        }
    }
}