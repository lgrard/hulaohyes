using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera
{
    public class CameraElement
    {
        private Camera _camera;
        private CinemachineVirtualCamera _virtualCamera;

        public CameraElement(Camera pCamera, CinemachineVirtualCamera pVirtualCamera)
        {
            _camera = pCamera;
            _virtualCamera = pVirtualCamera;
        }

        public Transform Target
        {
            get => _virtualCamera.Follow;
            set => _virtualCamera.Follow = value;
        }

        public Transform Camera
        {
            get => _camera.transform;
        }

        public float ScreenPosX
        {
            get => _camera.rect.x;
            set => _camera.rect = new Rect(value, 0, 1, 1);
        }
    }
}