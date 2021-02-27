using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class AngledCameraElement : CameraElement
    {
        public AngledCameraElement(Camera pCamera, CinemachineVirtualCamera pAngledCamGlobal,
            Transform pPlayerGroup, Transform pTarget, float pScreenOffset, float pTargetAngle, float pDistance ,bool pIsDollyToFit):base(pCamera,pTarget)
        {
            _camGlobal = pAngledCamGlobal;
            _camGlobal.transform.localEulerAngles = new Vector3(pTargetAngle, 0, 0);
            _camGlobal.Follow = pPlayerGroup;

            CinemachineFramingTransposer _angledFramingTransposer = _camGlobal.GetCinemachineComponent<CinemachineFramingTransposer>();
            _angledFramingTransposer.m_ScreenY = pScreenOffset;

            if (pIsDollyToFit)
            {
                _angledFramingTransposer.m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.HorizontalAndVertical;
                _angledFramingTransposer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.DollyOnly;
                _angledFramingTransposer.m_GroupFramingSize = 0.5f;
                _angledFramingTransposer.m_MinimumDistance = pDistance;
            }

            else
            {
                _angledFramingTransposer.m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.None;
                _angledFramingTransposer.m_CameraDistance = pDistance;
            }
        }
    }
}
