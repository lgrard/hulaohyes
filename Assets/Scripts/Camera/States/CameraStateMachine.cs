using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;
using hulaohyes.camera.elements;

namespace hulaohyes.camera.states
{
    public class CameraStateMachine : StateMachine
    {
        private Splitted _splitted;
        private Merged _merged;

        public CameraStateMachine(CameraElement pCamElement0, CameraElement pCamElement1,
            SideCameraElement pSideCamElement0, SideCameraElement pSideCamElement1, RectTransform pMask0, RectTransform pMask1, RectTransform pBar) :base()
        {
            _splitted = new Splitted(this, pCamElement0,pCamElement1, pSideCamElement0, pSideCamElement1, pMask0,pMask1,pBar);
            _merged = new Merged(this, pCamElement0, pCamElement1);
            CurrentState = _merged;
        }

        /// Returns this StateMachine splitted state
        public Splitted SplitState { get => _splitted; }

        /// Returns this StateMachine merged state
        public Merged MergeState { get => _merged; }
    }
}
