using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;
using hulaohyes.camera.elements;
using Cinemachine;

namespace hulaohyes.camera.states
{
    public class Splitted : CameraState
    {
        private const float RAD2DEG = 180 / Mathf.PI;
        private const float DEG2RAD = Mathf.PI / 180;
        private const float CENTER_MAGNITUDE = 0.25f;
        private const float MERGE_THRESHOLD = 10;

        private CameraManager _camManager;
        private CinemachineFramingTransposer _sideCam0transposer;
        private CinemachineFramingTransposer _sideCam1transposer;
        private SideCameraElement _sideCameraElement0;
        private SideCameraElement _sideCameraElement1;
        private CinemachineComposer _tdCam0Composer;
        private CinemachineComposer _tdCam1Composer;
        private RectTransform _mask0;
        private RectTransform _mask1;
        private RectTransform _bar;
        private Camera _cam1;
        private float _splitAngle;

        public Splitted(CameraStateMachine pStateMachine, CameraElement pCamElement0, CameraElement pCamElement1,
            SideCameraElement pSideCamElement0, SideCameraElement pSideCamElement1, RectTransform pMask0, RectTransform pMask1, RectTransform pBar)
            : base(pStateMachine, pCamElement0, pCamElement1)
        {
            _cam1 = pSideCamElement1.Camera;
            _camManager = CameraManager.getInstance();
            _sideCameraElement0 = pSideCamElement0;
            _sideCameraElement1 = pSideCamElement1;
            _sideCam0transposer = pSideCamElement0.SideCamTransposer;
            _sideCam1transposer = pSideCamElement1.SideCamTransposer;
            _mask0 = pMask0;
            _mask1 = pMask1;
            _bar = pBar;
        }

        IEnumerator SplitCams()
        {
            SetUIAngle();

            yield return new WaitForEndOfFrame();

            _cam1.enabled = true;
            _mask1.gameObject.SetActive(true);
            _mask0.gameObject.SetActive(true);
            _bar.gameObject.SetActive(true);

            UpdateCamGlobalPriority(_camElement0, 1);
            UpdateCamGlobalPriority(_camElement1, 0);
            UpdateCamNormalPriority(_camElement0, 12);
            UpdateCamNormalPriority(_camElement1, 11);
        }

        IEnumerator MergeCams()
        {
            UpdateCamGlobalPriority(_camElement0, 31);
            UpdateCamGlobalPriority(_camElement1, 30);
            UpdateCamNormalPriority(_camElement0, 10);
            UpdateCamNormalPriority(_camElement1, 9);

            yield return new WaitForSeconds(0.5f);

            _cam1.enabled = false;
            _mask1.gameObject.SetActive(false);
            _mask0.gameObject.SetActive(false);
            _bar.gameObject.SetActive(false);
        }

        void SetScreenPosition()
        {
            var lScreenX0 = (Mathf.Cos(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f;
            var lScreenY0 = (Mathf.Sin(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f;
            var lScreenX1 = 1 - ((Mathf.Cos(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f);
            var lScreenY1 = 1 - ((Mathf.Sin(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f);

            if (_camElement0 is SideCameraElement)
            {
                _sideCam0transposer.m_ScreenX = lScreenX0;
                _sideCam0transposer.m_ScreenY = lScreenY0;
            }

            else if (_camElement0 is TopDownCameraElement)
            {
                _tdCam0Composer.m_ScreenX = lScreenX0;
                _tdCam0Composer.m_ScreenY = lScreenY0;
            }

            if (_camElement1 is SideCameraElement)
            {
                _sideCam1transposer.m_ScreenX = lScreenX1;
                _sideCam1transposer.m_ScreenY = lScreenY1;
            }

            else if (_camElement1 is TopDownCameraElement)
            {
                _tdCam1Composer.m_ScreenX = lScreenX1;
                _tdCam1Composer.m_ScreenY = lScreenY1;
            }
        }

        void SetUIAngle()
        {
            _splitAngle = Mathf.Atan2(_playerDistance.x, _playerDistance.z) * RAD2DEG - 90;
            _bar.localEulerAngles = new Vector3(0, 0, -_splitAngle);
            _mask0.localEulerAngles = new Vector3(0, 0, -_splitAngle);
            _mask1.localEulerAngles = new Vector3(0, 0, -_splitAngle - 180);
        }

        public void UpdateCamElement(int pIndex, CameraElement pCamElement)
        {
            if (pIndex == 0)
            {
                UpdateCamGlobalPriority(_camElement0, 0);
                UpdateCamNormalPriority(_camElement0, 0);
                _camElement0 = pCamElement;
                UpdateCamGlobalPriority(_camElement0, 99);
                UpdateCamNormalPriority(_camElement0, 100);
            }

            else if (pIndex == 1)
            {
                UpdateCamGlobalPriority(_camElement1, 0);
                UpdateCamNormalPriority(_camElement1, 0);
                _camElement1 = pCamElement;
                UpdateCamGlobalPriority(_camElement1, 99);
                UpdateCamNormalPriority(_camElement1, 100);
            }
        }

        public void SetTDCamComposer(int pIndex, TopDownCameraElement pTDCameraElement)
        {
            if (pIndex == 0) _tdCam0Composer = pTDCameraElement.TDCamTransposer;
            if (pIndex == 1) _tdCam1Composer = pTDCameraElement.TDCamTransposer;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _camManager.StartCoroutine(SplitCams());
        }

        public override void LoopLogic()
        {
            base.LoopLogic();

            if (_playerDistance.magnitude < MERGE_THRESHOLD) _stateMachine.CurrentState = _stateMachine.MergedState;

            SetUIAngle();
            SetScreenPosition();
        }

        public override void OnExit()
        {
            base.OnExit();
            _camManager.StartCoroutine(MergeCams());
        }
    }
}
