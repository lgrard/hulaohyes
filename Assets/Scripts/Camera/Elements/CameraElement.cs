using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class CameraElement
    {
        private Camera _camera;
        protected Transform _target;
        protected CinemachineVirtualCamera _camNormal;
        protected CinemachineVirtualCamera _camGlobal;

        public CameraElement(Camera pCamera, Transform pTarget)
        {
            _target = pTarget;
            _camera = pCamera;
        }

        public Vector3 TargetPosition
        {
            get => _target.transform.position;
        }

        public Camera Camera
        {
            get => _camera;
        }

        public CinemachineVirtualCamera CamNormal
        {
            get => _camNormal;
        }
        public CinemachineVirtualCamera CamGlobal
        {
            get => _camGlobal;
        }
    }
}