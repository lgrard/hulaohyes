﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using hulaohyes;
using hulaohyes.camera.states;
using hulaohyes.camera.elements;

namespace hulaohyes.camera
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _instance;

        [Header("Camera 0")]
        [SerializeField] Camera _cam0;
        [SerializeField] CinemachineVirtualCamera _sideCam0;
        [SerializeField] CinemachineVirtualCamera _sideGlobalCam0;

        [Header("Camera 1")]
        [SerializeField] Camera _cam1;
        [SerializeField] CinemachineVirtualCamera _sideCam1;
        [SerializeField] CinemachineVirtualCamera _sideGlobalCam1;

        [Header("DepthMasks")]
        [SerializeField] RectTransform _mask0;
        [SerializeField] RectTransform _mask1;

        [Header("Splitting bar")]
        [SerializeField] RectTransform _bar;

        [Header("Targets")]
        private Transform _playerGroup;
        private Transform _player0;
        private Transform _player1;

        private CameraStateMachine _stateMachine;
        private SideCameraElement _sideCamElement0;
        private SideCameraElement _sideCamElement1;


        private void Awake()
        {
            if (_instance == null)  _instance = this;

            else
            {
                Debug.LogError("Attempt to create a second CameraManager");
                Destroy(this.gameObject);
            }

            Init();
        }

        void Init()
        {
            _player0 = GameManager.getPlayer(0).transform;
            _player1 = GameManager.getPlayer(1).transform;
            _playerGroup = FindObjectOfType<CinemachineTargetGroup>().transform;

            _sideCamElement0 = new SideCameraElement(_cam0, _sideCam0, _sideGlobalCam0,_player0, _playerGroup);
            _sideCamElement1 = new SideCameraElement(_cam1, _sideCam1, _sideGlobalCam1,_player1, _sideGlobalCam0.transform);

            _stateMachine = new CameraStateMachine(_sideCamElement0, _sideCamElement1, _sideCamElement0,_sideCamElement1,_mask0, _mask1, _bar);
        }

        /// Returns CameraManager singleton instance
        /// <returns>CameraManager singleton instance</returns>
        public static CameraManager getInstance()
        {
            if (_instance == null) _instance = new CameraManager();
            return _instance;
        }

        /// Assign a new global camera by changing camera priorities
        /// <param name="pAltCamElement">New global alternative camera</param>
        public void SetAltCameraElement(CameraElement pAltCamElement)
        {
            _stateMachine.SplitState.ForceMerge();
            _stateMachine.MergeState.SetCanSplit = false;
            _stateMachine.SplitState.UpdateCamGlobalPriority(_sideCamElement0, 0);
            _stateMachine.SplitState.UpdateCamNormalPriority(_sideCamElement0, 0);
            _stateMachine.SplitState.camElement0 = pAltCamElement;
            _stateMachine.SplitState.UpdateCamGlobalPriority(_stateMachine.SplitState.camElement0, 100);
        }

        /// Reset global and normal cameras to sideCamElements
        public void ResetCamerasElement()
        {
            _stateMachine.SplitState.UpdateCamGlobalPriority(_stateMachine.SplitState.camElement0, 0);
            _stateMachine.SplitState.camElement0 = _sideCamElement0;
            _stateMachine.SplitState.UpdateCamGlobalPriority(_sideCamElement0, 100);
            _stateMachine.SplitState.UpdateCamNormalPriority(_sideCamElement0, 99);
            _stateMachine.MergeState.SetCanSplit = true;
        }

        /// Returns associated player index Camera component
        /// <param name="pIndex">Associated Camera player index</param>
        /// <returns>Camera component</returns>
        public Camera GetCamera(int pIndex)
        {
            if (pIndex == 0) return _cam0;
            if (pIndex == 1) return _cam1;
            else
            {
                Debug.LogError("Invalid camera index");
                return null;
            }
        }

        /// Returns current global PlayerGroup
        public Transform PlayerGroup { get => _playerGroup; }

        private void Update() => _stateMachine.CurrentState.LoopLogic();
    }
}
