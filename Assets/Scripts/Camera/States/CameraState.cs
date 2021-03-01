using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;
using hulaohyes.camera.elements;

namespace hulaohyes.camera.states
{
    public class CameraState : State
    {
        public CameraElement camElement0;
        public CameraElement camElement1;
        protected CameraStateMachine _stateMachine;
        protected Vector3 _playerDistance;

        public CameraState(CameraStateMachine pStateMachine, CameraElement pCamElement0, CameraElement pCamElement1) : base()
        {
            _stateMachine = pStateMachine;
            camElement0 = pCamElement0;
            camElement1 = pCamElement1;
            _playerDistance = new Vector3(0, 0, 0);
        }

        /// Update merged camera priority
        /// <param name="pCamElement">Associated camera to update</param>
        /// <param name="pGlobalPriority">New merged camera priority</param>
        public void UpdateCamGlobalPriority(CameraElement pCamElement, int pGlobalPriority) => pCamElement.CamGlobal.m_Priority = pGlobalPriority;

        /// Update splitted camera priority
        /// <param name="pCamElement">Associated camera to update</param>
        /// <param name="pNormalPriority">New splitted camera priority</param>
        public void UpdateCamNormalPriority(CameraElement pCamElement, int pNormalPriority) => pCamElement.CamNormal.m_Priority = pNormalPriority;

        public override void LoopLogic()
        {
            base.LoopLogic();
            _playerDistance = camElement0.TargetPosition - camElement1.TargetPosition;
        }
    }
}
