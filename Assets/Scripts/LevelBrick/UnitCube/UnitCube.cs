using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.levelbrick.door;
using hulaohyes.player;

namespace hulaohyes.levelbrick.unitcube
{
    public class UnitCube : Pickable
    {
        private Slab currentSlab;
        private UnitCubeSpawner _currentSpawner;
        private LayerMask killZoneLayer;

        protected override void Init()
        {
            base.Init();
            killZoneLayer = LayerMask.NameToLayer("KillZone");
        }

        public void DestroyUnitCube()
        {
            if(_currentPicker != null) _currentPicker.DropTarget();
            _currentSpawner.DestroyCurrentCube();
            Destroy(this.gameObject,0.5f);
        }

        private void FixedUpdate() => base.rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);

        /// Freeze this cube into place and sets its parent slab
        /// <param name="pSlab"> Associated parent slab </param>
        public void FreezeCube(Slab pSlab)
        {
            rb.isKinematic = true;
            currentSlab = pSlab;
        }

        ///  Set this unit cube's current spawner
        public UnitCubeSpawner CurrentSpawner { set => _currentSpawner = value; }

        override public void GetPicked(PlayerController pPlayer)
        {
            base.GetPicked(pPlayer);
            if (currentSlab != null)
            {
                currentSlab.CurrentUnitCube = null;
                currentSlab = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == killZoneLayer) DestroyUnitCube();
        }
    }
}
