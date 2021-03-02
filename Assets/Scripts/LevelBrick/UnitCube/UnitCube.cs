using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.unitcube
{
    public class UnitCube : Pickable
    {
        private UnitCubeSpawner _currentSpawner;
        private LayerMask _killZoneLayer;

        protected override void Init()
        {
            base.Init();
            _killZoneLayer = LayerMask.NameToLayer("KillZone");
        }

        void DestroyUnitCube()
        {
            _currentSpawner.DestroyCurrentCube();
            Destroy(this.gameObject,1f);
        }

        private void FixedUpdate() => base._rb.AddForce(Physics.gravity * 4, ForceMode.Acceleration);

        public UnitCubeSpawner CurrentSpawner { set => _currentSpawner = value; }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _killZoneLayer) DestroyUnitCube();
        }
    }
}
