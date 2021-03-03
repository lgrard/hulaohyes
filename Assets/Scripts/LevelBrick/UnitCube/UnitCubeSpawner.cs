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
        private Vector3 _spawnOffset = new Vector3(0,1,0);
        private bool _canSpawnCube = true;
        [SerializeField] GameObject _spawner;
        [SerializeField] bool _drawGizmos = true;

        private void Start()
        {
            string lPath = "Prefabs/Bricks/UnitCube";
            _unitCubePrefab = Resources.Load(lPath) as GameObject;
            if (_unitCubePrefab == null) Debug.LogError(lPath +" is not a valid path");
            if (_spawner == null) Debug.LogError("You need to assign a spawner object to "+gameObject.name);
        }

        private IEnumerator ButtonTimer()
        {
            _canSpawnCube = false;
            yield return new WaitForSeconds(1f);

            GameObject lUnitcubeToSpawn = PrefabUtility.InstantiatePrefab(_unitCubePrefab) as GameObject;
            if (lUnitcubeToSpawn.TryGetComponent<UnitCube>(out UnitCube pCube)) _currentUnitCube = pCube;
            _currentUnitCube.transform.position = _spawner.transform.position + _spawnOffset;
            _currentUnitCube.CurrentSpawner = this;

            _canSpawnCube = true;
        }

        public void PushButton()
        {
            if (_currentUnitCube != null) _currentUnitCube.DestroyUnitCube();
            else if (_canSpawnCube) StartCoroutine(ButtonTimer());
        }

        /// Nullify reference to old cube and create a new one
        public void DestroyCurrentCube()
        {
            _currentUnitCube = null;
            if(_canSpawnCube) StartCoroutine(ButtonTimer());
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(this.transform.position, 0.25f);

                Gizmos.color = Color.green;
                if (_currentUnitCube != null) Gizmos.DrawLine(this.transform.position, _currentUnitCube.transform.position);

                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
               if(_spawner!=null) Gizmos.DrawCube(_spawner.transform.localPosition + _spawnOffset, Vector3.one);
            }
        }
    }
}
