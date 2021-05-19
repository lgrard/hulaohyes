using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using hulaohyes.camera.states;
using hulaohyes.camera.elements;


namespace hulaohyes.camera
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _instance;

        [Header("Camera 0")]
        [SerializeField] Camera _cam0;
        [SerializeField] CinemachineVirtualCamera sideCam0;
        [SerializeField] CinemachineVirtualCamera sideGlobalCam0;
        [SerializeField] GameObject cam0Volume;

        [Header("Camera 1")]
        [SerializeField] Camera _cam1;
        [SerializeField] CinemachineVirtualCamera sideCam1;
        [SerializeField] CinemachineVirtualCamera sideGlobalCam1;

        [Header("DepthMasks")]
        [SerializeField] RectTransform mask0;
        [SerializeField] RectTransform mask1;

        [Header("Splitting bar")]
        [SerializeField] RectTransform bar;

        [Header("Targets")]
        private Transform playerGroup;
        private Transform player0;
        private Transform player1;

        private CameraStateMachine stateMachine;
        private SideCameraElement sideCamElement0;
        private SideCameraElement sideCamElement1;


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
            player0 = GameManager.getPlayer(0).transform;
            player1 = GameManager.getPlayer(1).transform;
            CinemachineTargetGroup lPlayerTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
            lPlayerTargetGroup.AddMember(player0,1,1);
            lPlayerTargetGroup.AddMember(player1,1,1);
            playerGroup = lPlayerTargetGroup.transform;

            sideCamElement0 = new SideCameraElement(_cam0, sideCam0, sideGlobalCam0,player0, playerGroup);
            sideCamElement1 = new SideCameraElement(_cam1, sideCam1, sideGlobalCam1,player1, sideGlobalCam0.transform);

            stateMachine = new CameraStateMachine(sideCamElement0, sideCamElement1, sideCamElement0,sideCamElement1,mask0, mask1, bar, cam0Volume);
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
            stateMachine.SplitState.ForceMerge();
            stateMachine.MergeState.SetCanSplit = false;
            stateMachine.SplitState.UpdateCamGlobalPriority(sideCamElement0, 0);
            stateMachine.SplitState.UpdateCamNormalPriority(sideCamElement0, 0);
            stateMachine.SplitState.camElement0 = pAltCamElement;
            stateMachine.SplitState.UpdateCamGlobalPriority(stateMachine.SplitState.camElement0, 100);
        }

        /// Reset global and normal cameras to sideCamElements
        public void ResetCamerasElement()
        {
            stateMachine.SplitState.UpdateCamGlobalPriority(stateMachine.SplitState.camElement0, 0);
            stateMachine.SplitState.camElement0 = sideCamElement0;
            stateMachine.SplitState.UpdateCamGlobalPriority(sideCamElement0, 100);
            stateMachine.SplitState.UpdateCamNormalPriority(sideCamElement0, 99);
            stateMachine.MergeState.SetCanSplit = true;
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
        public Transform PlayerGroup { get => playerGroup; }

        private void Update() => stateMachine.CurrentState.LoopLogic();
    }
}
