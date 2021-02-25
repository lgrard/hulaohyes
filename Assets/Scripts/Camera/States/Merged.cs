using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.camera.elements;

namespace hulaohyes.camera.states
{
    public class Merged : CameraState
    {
        private const float SPLIT_THRESHOLD = 14;

        public Merged(CameraStateMachine pStateMachine, CameraElement pCamElement0, CameraElement pCamElement1) : base(pStateMachine, pCamElement0, pCamElement1) { }

        public override void LoopLogic()
        {
            if (_playerDistance.magnitude > SPLIT_THRESHOLD) _stateMachine.CurrentState = _stateMachine.SplittedState;
            base.LoopLogic();
        }
    }
}
