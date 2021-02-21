using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraVoronoiManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] Camera cam0;
    [SerializeField] Camera cam1;

    [SerializeField] RawImage mask0;
    [SerializeField] RawImage mask1;

    [SerializeField] GameObject player0;
    [SerializeField] GameObject player1;
    [SerializeField] RawImage bar;

    [Header("Values")]
    private float _fov = 60;
    private float _splitAngle;

    private void Start()
    {
        cam0.fieldOfView = _fov;
        cam1.fieldOfView = _fov;
    }

    private void Update()
    {
        Vector3 lDistance = player1.transform.position - player0.transform.position;
        _splitAngle = Mathf.Atan2(lDistance.x, lDistance.z)*180/Mathf.PI + 90;

        bar.rectTransform.eulerAngles = new Vector3 (0,0,-_splitAngle);
        mask0.rectTransform.eulerAngles = new Vector3(0, 0, -_splitAngle);
        mask1.rectTransform.eulerAngles = new Vector3(0, 0, -_splitAngle - 180);
    }

}
