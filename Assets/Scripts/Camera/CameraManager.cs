using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace hulaohyes.camera
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager _instance;

        [SerializeField] Transform _player1;
        [SerializeField] Transform _player2;
        [SerializeField] Transform _playerGroup;

        [Header("Cameras")]
        [SerializeField] Camera _leftCam;
        [SerializeField] Camera _rightCam;
        [SerializeField] CinemachineVirtualCamera _leftVirtualCam;
        [SerializeField] CinemachineVirtualCamera _rightVirtualCam;
        private CameraElement _leftCamElement;
        private CameraElement _rightCamElement;

        [Header("Merge/split distance thresholds")]
        [Tooltip("Distance between camera before they merge")]
        [SerializeField] float _mergeThreshold = 10;
        [Tooltip("Distance between camera before they split")]
        [SerializeField] float _splitThreshold = 30;

        bool _merged = false;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else
            {
                Debug.LogError("Attempt to create a second CameraManager");
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            _leftCamElement = new CameraElement(_leftCam, _leftVirtualCam);
            _rightCamElement = new CameraElement(_rightCam, _rightVirtualCam);
        }

        private void Update()
        {
            if (canMerge)
            {
                Debug.Log("Camera merge");
                ChangeCameraState(0, _playerGroup, _player2);
                _merged = true;
            }

            if (canSplit)
            {
                Debug.Log("Camera split");
                List<Transform> lSideCheck = SideCheck();
                ChangeCameraState(-0.5f, lSideCheck[0], lSideCheck[1]);
                _merged = false;
            }
        }

        bool canMerge => Vector3.Distance(_player1.transform.position, _player2.transform.position) < _mergeThreshold && !_merged;
        bool canSplit => Vector3.Distance(_player1.transform.position, _player2.transform.position) > _splitThreshold && _merged;

        void ChangeCameraState(float pScreenPosX, Transform pLeftTarget, Transform pRightTarget)
        {
            _leftCamElement.Target = pLeftTarget;
            _rightCamElement.Target = pRightTarget;

            _leftCamElement.ScreenPosX = pScreenPosX;
        }

        List<Transform> SideCheck()
        {
            List<Transform> lSideOrder = new List<Transform>();

            if(_player1.transform.position.x < _player2.transform.position.x)
            {
                lSideOrder.Add(_player2);
                lSideOrder.Add(_player1);
            }

            else
            {
                lSideOrder.Add(_player1);
                lSideOrder.Add(_player2);
            }

            return lSideOrder;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_playerGroup.position, _splitThreshold/2);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_playerGroup.position, _mergeThreshold/2);
        }
    }
}
