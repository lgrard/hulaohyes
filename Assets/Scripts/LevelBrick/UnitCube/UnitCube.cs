﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.levelbrick.door;

namespace hulaohyes.levelbrick.unitcube
{
    public class UnitCube : Pickable
    {
        private Slab _currentSlab;
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

        /// Freeze this cube into place and sets its parent slab
        /// <param name="pSlab"> Associated parent slab </param>
        public void FreezeCube(Slab pSlab)
        {
            _rb.isKinematic = true;
            _currentSlab = pSlab;
        }

        ///  Set this unit cube's current spawner
        public UnitCubeSpawner CurrentSpawner { set => _currentSpawner = value; }

        override protected private void GetPicked()
        {
            base.GetPicked();
            if (_currentSlab != null)
            {
                _currentSlab.CurrentUnitCube = null;
                _currentSlab = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _killZoneLayer) DestroyUnitCube();
        }
    }
}
