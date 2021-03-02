using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace hulaohyes.levelbrick.unitcube
{
    public class UnitCubeSpawner : MonoBehaviour
    {
        private UnitCube _currentUnitCube;
        private GameObject _unitCubePrefab;
        [SerializeField] bool _drawGizmos = true;

        private void Start()
        {
            string lPath = "Prefabs/Bricks/UnitCube";
            _unitCubePrefab = Resources.Load(lPath) as GameObject;
            if (_unitCubePrefab != null) CreateCube();
            else Debug.Log(lPath +" is not a valid path");
        }

        private void CreateCube()
        {
            GameObject lUnitcubeToSpawn = PrefabUtility.InstantiatePrefab(_unitCubePrefab) as GameObject;
            if(lUnitcubeToSpawn.TryGetComponent<UnitCube>(out UnitCube pCube)) _currentUnitCube = pCube;
            _currentUnitCube.transform.position = this.transform.position;
            _currentUnitCube.CurrentSpawner = this;
        }

        public void DestroyCurrentCube()
        {
            _currentUnitCube = null;
            CreateCube();
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(this.transform.position, 0.25f);

                if (_currentUnitCube != null) Gizmos.DrawLine(this.transform.position, _currentUnitCube.transform.position);
            }
        }
    }
}
