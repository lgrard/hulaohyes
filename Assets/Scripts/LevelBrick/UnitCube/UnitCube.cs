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
        private MeshRenderer renderer;

        protected override void Init()
        {
            base.Init();
            renderer = GetComponent<MeshRenderer>();
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
            if (_currentPicker != null) _currentPicker.DropTarget();
            renderer.materials[0].SetColor("EmitColor", currentSlab.Color);
            renderer.materials[0].SetFloat("Brightness", 5);
        }

        ///  Set this unit cube's current spawner
        public UnitCubeSpawner CurrentSpawner { set => _currentSpawner = value; }

        override public void GetPicked(PlayerController pPlayer)
        {
            base.GetPicked(pPlayer);

            if (currentSlab != null)
            {
                renderer.materials[0].SetColor("EmitColor", Color.white);
                renderer.materials[0].SetFloat("Brightness", 0);
                currentSlab.SetLed(this, 0);
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
