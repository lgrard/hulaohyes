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
        private MeshRenderer renderer;

        protected override void Init()
        {
            base.Init();
            renderer = GetComponent<MeshRenderer>();
        }

        public void DestroyUnitCube()
        {
            if(_currentPicker != null) _currentPicker.DropTarget();
            if(currentSlab != null)
            {
                currentSlab.SetLed(this, 0);
                currentSlab.CurrentUnitCube = null;
                currentSlab = null;
            }
            _currentSpawner.DestroyCurrentCube();
            Destroy(this.gameObject,0.5f);
        }

        /// Freeze this cube into place and sets its parent slab
        /// <param name="pSlab"> Associated parent slab </param>
        public void FreezeCube(Slab pSlab)
        {
            rb.isKinematic = true;
            currentSlab = pSlab;
            if (_currentPicker != null)
            {
                _currentPicker.DropTarget();
                _currentPicker = null;
            }
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

        protected override void Drowns()
        {
            DestroyUnitCube();
            base.Drowns();
        }
    }
}
