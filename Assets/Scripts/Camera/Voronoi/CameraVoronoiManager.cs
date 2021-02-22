using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraVoronoiManager : MonoBehaviour
{
    private const float RAD2DEG = 180 / Mathf.PI;
    private const float DEG2RAD = Mathf.PI / 180;
    private const float CENTER_MAGNITUDE = 0.25f;
    private const float SPLIT_THRESHOLD = 3;
    private const float MERGE_THRESHOLD = 1;

    [Header("Cameras")]
    [SerializeField] Camera cam0;
    [SerializeField] Camera cam1;
    [SerializeField] CinemachineVirtualCamera sideCam0;
    [SerializeField] CinemachineVirtualCamera sideCam1;
    [SerializeField] CinemachineVirtualCamera sideGlobalCam0;
    [SerializeField] CinemachineVirtualCamera sideGlobalCam1;
    private CinemachineFramingTransposer sideCam0transposer;
    private CinemachineFramingTransposer sideCam1transposer;

    [Header("DepthMasks")]
    [SerializeField] RectTransform mask0;
    [SerializeField] RectTransform mask1;

    [Header("Targets")]
    [SerializeField] Transform player0;
    [SerializeField] Transform player1;
    [SerializeField] Transform playerGroup;

    [Header("Splitting bar")]
    [SerializeField] RectTransform bar;

    [Header("Values")]
    private float _splitAngle;
    private Vector3 _playerDistance;

    bool isMerged = false;

    private void Start()
    {
        sideCam0.Follow = player0;
        sideCam1.Follow = player1;

        sideCam0transposer = sideCam0.GetCinemachineComponent<CinemachineFramingTransposer>();
        sideCam1transposer = sideCam1.GetCinemachineComponent<CinemachineFramingTransposer>();
        sideCam0transposer.m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.None;
        sideCam1transposer.m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.None;
    }

    private void Update()
    {
        _playerDistance = player1.position - player0.position;
        
        if (!isMerged)
        {
            _splitAngle = Mathf.Atan2(_playerDistance.x, _playerDistance.z) * RAD2DEG + 90;
            bar.localEulerAngles = new Vector3(0, 0, -_splitAngle);
            mask0.localEulerAngles= new Vector3(0, 0, -_splitAngle);
            mask1.localEulerAngles = new Vector3(0, 0, -_splitAngle - 180);

            sideCam0transposer.m_ScreenX = (Mathf.Cos(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE)+0.5f;
            sideCam0transposer.m_ScreenY = (Mathf.Sin(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE)+0.5f;

            sideCam1transposer.m_ScreenX = 1-((Mathf.Cos(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE)+0.5f);
            sideCam1transposer.m_ScreenY = 1-((Mathf.Sin(_splitAngle * DEG2RAD) * CENTER_MAGNITUDE)+0.5f);
        }

        SplitCheck();   
    }
    private void SplitCheck()
    {
        if (_playerDistance.magnitude < MERGE_THRESHOLD && !isMerged)
            StartCoroutine(MergeCams());
        if (_playerDistance.magnitude > SPLIT_THRESHOLD && isMerged)
            SplitCams();
    }

    IEnumerator MergeCams()
    {
        sideGlobalCam0.m_Priority = 30;
        sideGlobalCam1.m_Priority = 31;
        sideCam0.m_Priority = 9;
        sideCam1.m_Priority = 10;

        yield return new WaitForSeconds(0.5f);

        cam1.enabled = false;
        mask1.gameObject.SetActive(false);
        mask0.gameObject.SetActive(false);
        bar.gameObject.SetActive(false);
        
        isMerged = true;
    }

    void SplitCams()
    {
        _splitAngle = Mathf.Atan2(_playerDistance.x, _playerDistance.z) * RAD2DEG + 90;
        bar.localEulerAngles = new Vector3(0, 0, -_splitAngle);
        
        isMerged = false;

        cam1.enabled = true;
        mask1.gameObject.SetActive(true);
        mask0.gameObject.SetActive(true);
        bar.gameObject.SetActive(true);

        sideGlobalCam0.m_Priority = 0;
        sideGlobalCam1.m_Priority = 1;
        sideCam0.m_Priority = 11;
        sideCam1.m_Priority = 12;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerGroup.position, SPLIT_THRESHOLD/2);
        Gizmos.DrawWireSphere(playerGroup.position, MERGE_THRESHOLD/2);
    }
}
