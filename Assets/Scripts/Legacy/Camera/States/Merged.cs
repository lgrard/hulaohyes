using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.camera.elements;

namespace hulaohyes.camera.states
{
    public class Merged : CameraState
    {
        private const float SPLIT_THRESHOLD = 14;
        private bool _canSplit = true;

        public Merged(CameraStateMachine pStateMachine, CameraElement pCamElement0, CameraElement pCamElement1) : base(pStateMachine, pCamElement0, pCamElement1) { }

        /// Allows/forbid splitting
        public bool SetCanSplit { set => _canSplit = value; }

        bool CanSplit => _playerDistance.magnitude > SPLIT_THRESHOLD && _canSplit;

        public override void LoopLogic()
        {
            if (CanSplit) _stateMachine.CurrentState = _stateMachine.SplitState;
            base.LoopLogic();
        }
    }
}
