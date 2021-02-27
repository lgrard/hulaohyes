using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class LookAtCameraElement : CameraElement
    {
        public LookAtCameraElement(Camera pCamera,CinemachineVirtualCamera pLookAtCamGlobal, Transform pTarget, Transform pPlayerGroup, Transform pPosition)
            : base(pCamera, pTarget)
        {
            _camGlobal = pLookAtCamGlobal;
            _camGlobal.Follow = pPosition;
            _camGlobal.LookAt = pPlayerGroup;
        }
    }
}