using System.Collections;
using UnityEngine;

namespace hulaohyes.Assets.Scripts.Actionables
{
    public class UnitCubeSpawner : Actionable
    {
        [SerializeField] private GameObject UnitCubePrefab = null;
        [SerializeField] private Transform SpawnPoint = null;
        private GameObject currentCube;

        public override void DoAction()
        {
            base.DoAction();
            SpawnCube();
        }

        public override void CancelAction()
        {
            base.CancelAction();
        }

        private void SpawnCube()
        {
            if (currentCube != null)
            {
                Destroy(currentCube);
            }

            currentCube = GameObject.Instantiate(UnitCubePrefab);
            currentCube.transform.parent = SpawnPoint;
            currentCube.transform.transform.localPosition = Vector3.zero;
            currentCube.name = "UnitCube";
        }
    }
}