using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera.elements
{
    public class CameraElement
    {
        private Camera _camera;
        protected Transform target;
        protected CinemachineVirtualCamera _camNormal;
        protected CinemachineVirtualCamera _camGlobal;

        public CameraElement(Camera pCamera, Transform pTarget)
        {
            target = pTarget;
            _camera = pCamera;
        }

        /// Returns associated target position
        public Vector3 TargetPosition
        {
            get => target.transform.position;
        }

        /// Returns associated camera
        public Camera Camera
        {
            get => _camera;
        }

        /// Returns associated splitted virtual camera
        public CinemachineVirtualCamera CamNormal
        {
            get => _camNormal;
        }

        /// Returns associated merged virtual camera
        public CinemachineVirtualCamera CamGlobal
        {
            get => _camGlobal;
        }
    }
}