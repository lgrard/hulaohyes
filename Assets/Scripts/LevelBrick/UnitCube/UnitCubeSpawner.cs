using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace hulaohyes.levelbrick.unitcube
{
    public class UnitCubeSpawner : MonoBehaviour
    {
        private UnitCube currentUnitCube;
        private GameObject unitCubePrefab;
        private Vector3 spawnOffset = new Vector3(0,1,0);
        private bool canSpawnCube = true;
        [SerializeField] GameObject spawner;
        [SerializeField] bool drawGizmos = true;

        private void Start()
        {
            string lPath = "Prefabs/Bricks/UnitCube";
            unitCubePrefab = Resources.Load(lPath) as GameObject;
            if (unitCubePrefab == null) Debug.LogError(lPath +" is not a valid path");
            if (spawner == null) Debug.LogError("You need to assign a spawner object to "+gameObject.name);
        }

        private IEnumerator ButtonTimer()
        {
            canSpawnCube = false;
            yield return new WaitForSeconds(1f);

            GameObject lUnitcubeToSpawn = PrefabUtility.InstantiatePrefab(unitCubePrefab) as GameObject;
            if (lUnitcubeToSpawn.TryGetComponent<UnitCube>(out UnitCube pCube)) currentUnitCube = pCube;
            currentUnitCube.transform.position = spawner.transform.position + spawnOffset;
            currentUnitCube.CurrentSpawner = this;

            canSpawnCube = true;
        }

        public void PushButton()
        {
            if (currentUnitCube != null) currentUnitCube.DestroyUnitCube();
            else if (canSpawnCube) StartCoroutine(ButtonTimer());
        }

        /// Nullify reference to old cube and create a new one
        public void DestroyCurrentCube()
        {
            currentUnitCube = null;
            if(canSpawnCube) StartCoroutine(ButtonTimer());
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(this.transform.position, 0.25f);

                Gizmos.color = Color.green;
                if (currentUnitCube != null) Gizmos.DrawLine(this.transform.position, currentUnitCube.transform.position);

                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
               if(spawner!=null) Gizmos.DrawCube(spawner.transform.localPosition + spawnOffset, Vector3.one);
            }
        }
    }
}
