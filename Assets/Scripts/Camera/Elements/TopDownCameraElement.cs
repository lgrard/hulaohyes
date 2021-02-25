using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class TopDownCameraElement : CameraElement
    {
        public TopDownCameraElement(Camera pCamera,CinemachineVirtualCamera pTDCamGlobal, Transform pTarget, Transform pPlayerGroup, Transform pPosition)
            : base(pCamera, pTarget)
        {
            _camGlobal = pTDCamGlobal;
            _camGlobal.Follow = pPosition;
            _camGlobal.LookAt = pPlayerGroup;
        }
    }
}