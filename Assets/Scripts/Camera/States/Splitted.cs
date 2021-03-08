using System.Collections;
using UnityEngine;
using hulaohyes.camera.elements;
using Cinemachine;
using UnityEngine.Rendering;

namespace hulaohyes.camera.states
{
    public class Splitted : CameraState
    {
        private const float RAD2DEG = 180 / Mathf.PI;
        private const float DEG2RAD = Mathf.PI / 180;
        private const float CENTER_MAGNITUDE = 0.25f;
        private const float MERGE_THRESHOLD = 10;

        private CameraManager camManager;
        private CinemachineFramingTransposer sideCam0transposer;
        private CinemachineFramingTransposer sideCam1transposer;
        private RectTransform mask0;
        private RectTransform mask1;
        private RectTransform bar;
        private Camera cam1;
        private GameObject cam0Volume;
        private float splitAngle;

        public Splitted(CameraStateMachine pStateMachine, CameraElement pCamElement0, CameraElement pCamElement1,
            SideCameraElement pSideCamElement0, SideCameraElement pSideCamElement1, RectTransform pMask0, RectTransform pMask1, RectTransform pBar, GameObject pCam0Volume)
            : base(pStateMachine, pCamElement0, pCamElement1)
        {
            cam1 = pSideCamElement1.Camera;
            camManager = CameraManager.getInstance();
            sideCam0transposer = pSideCamElement0.SideCamTransposer;
            sideCam1transposer = pSideCamElement1.SideCamTransposer;
            mask0 = pMask0;
            mask1 = pMask1;
            bar = pBar;
            cam0Volume = pCam0Volume;
        }

        IEnumerator SplitCams()
        {
            SetUIAngle();

            yield return new WaitForEndOfFrame();

            cam1.enabled = true;
            mask1.gameObject.SetActive(true);
            mask0.gameObject.SetActive(true);
            bar.gameObject.SetActive(true);
            cam0Volume.SetActive(false);

            UpdateCamGlobalPriority(camElement0, 1);
            UpdateCamGlobalPriority(camElement1, 0);
            if (camElement0 is SideCameraElement) UpdateCamNormalPriority(camElement0, 12);
            if (camElement1 is SideCameraElement) UpdateCamNormalPriority(camElement1, 11);
        }

        /// Force camera merging and global camera usage
        public void ForceMerge() => _stateMachine.CurrentState = _stateMachine.MergeState;

        IEnumerator MergeCams()
        {
            UpdateCamGlobalPriority(camElement0, 31);
            UpdateCamGlobalPriority(camElement1, 30);
            if (camElement0 is SideCameraElement) UpdateCamNormalPriority(camElement0, 10);
            if (camElement1 is SideCameraElement) UpdateCamNormalPriority(camElement1, 9);

            yield return new WaitForSeconds(0.5f);

            cam0Volume.SetActive(true);
            cam1.enabled = false;
            mask1.gameObject.SetActive(false);
            mask0.gameObject.SetActive(false);
            bar.gameObject.SetActive(false);
        }

        void SetScreenPosition()
        {
            var lScreenX0 = (Mathf.Cos(splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f;
            var lScreenY0 = (Mathf.Sin(splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f;
            var lScreenX1 = 1 - ((Mathf.Cos(splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f);
            var lScreenY1 = 1 - ((Mathf.Sin(splitAngle * DEG2RAD) * CENTER_MAGNITUDE) + 0.5f);

            sideCam0transposer.m_ScreenX = lScreenX0;
            sideCam0transposer.m_ScreenY = lScreenY0;
            sideCam1transposer.m_ScreenX = lScreenX1;
            sideCam1transposer.m_ScreenY = lScreenY1;
        }

        void SetUIAngle()
        {
            splitAngle = Mathf.Atan2(_playerDistance.x, _playerDistance.z) * RAD2DEG - 90;
            bar.localEulerAngles = new Vector3(0, 0, -splitAngle);
            mask0.localEulerAngles = new Vector3(0, 0, -splitAngle);
            mask1.localEulerAngles = new Vector3(0, 0, -splitAngle - 180);
        }

        bool CanMerge => _playerDistance.magnitude < MERGE_THRESHOLD;

        public override void OnEnter()
        {
            base.OnEnter();
            camManager.StartCoroutine(SplitCams());
        }

        public override void LoopLogic()
        {
            base.LoopLogic();

            if (CanMerge) _stateMachine.CurrentState = _stateMachine.MergeState;

            SetUIAngle();
            SetScreenPosition();
        }

        public override void OnExit()
        {
            base.OnExit();
            camManager.StartCoroutine(MergeCams());
        }
    }
}
