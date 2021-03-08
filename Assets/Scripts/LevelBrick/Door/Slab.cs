using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using hulaohyes.levelbrick.unitcube;

namespace hulaohyes.levelbrick.door
{
    public class Slab : MonoBehaviour
    {
        private Color _color;
        private DoorManager _doorManager;
        private UnitCube _currentUnitCube;
        private PlayerController currentPlayer;
        private int currentUnits;
        private LayerMask collisionLayer;

        [Range(1,2)][SerializeField] int maxUnit = 2;
        [Tooltip("Where will the unit cube will magnet")]
        [SerializeField] Vector3 cubeSlotPosition;
        [Tooltip("TriggerSize")]
        [SerializeField] Vector3 triggerZoneSize;
        [Tooltip("Are the gizmos drawing fo this object")]
        [SerializeField] bool drawGizmos;

        void Start()
        {
            collisionLayer = LayerMask.GetMask("Player", "Bricks");
            CreateTrigger();
        }

        void CreateTrigger()
        {
            BoxCollider lTrigger = gameObject.AddComponent<BoxCollider>();
            lTrigger.size = triggerZoneSize;
            lTrigger.center = new Vector3(0, triggerZoneSize.y / 2, 0);
            lTrigger.isTrigger = true;
        }

        void MagnetCube(UnitCube pCube)
        {
            _currentUnitCube = pCube;
            _currentUnitCube.transform.parent = this.transform;
            _currentUnitCube.transform.localPosition = cubeSlotPosition;
            _currentUnitCube.transform.eulerAngles = Vector3.zero;
            _currentUnitCube.FreezeCube(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((collisionLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                if (currentUnits >= maxUnit) return;

                if(other.TryGetComponent<UnitCube>(out UnitCube pCube))
                {
                    if (_currentUnitCube == null) MagnetCube(pCube);
                    else return;
                }

                else if (other.TryGetComponent<PlayerController>(out PlayerController pPlayer))
                {
                    if (currentPlayer == null) currentPlayer = pPlayer;
                    else return;
                }

                currentUnits += 1;
                _doorManager.SetUnit(1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController pPlayer) && pPlayer == currentPlayer)
            {
                currentPlayer = null;
                currentUnits -= 1;
                _doorManager.SetUnit(-1);
            }
        }

        /// Set this Slab's color 
        public Color Color { set => _color = value; }

        /// Set this Slab's current door manager 
        public DoorManager DoorManager { set => _doorManager = value; }

        /// Set this Slab's current unit cube 
        public UnitCube CurrentUnitCube
        {
            set
            {
                if (value == null) currentUnits -= 1;
                _doorManager.SetUnit(-1);
                _currentUnitCube = value;
            }
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Vector3 lTriggerPosition = new Vector3(0, triggerZoneSize.y / 2, 0);
                Gizmos.DrawWireCube(lTriggerPosition, triggerZoneSize);
                Gizmos.color = Color.green;
                Vector3 lCubePosition = new Vector3(cubeSlotPosition.x, cubeSlotPosition.y+0.55f, cubeSlotPosition.z);
                Gizmos.DrawWireCube(lCubePosition, Vector3.one * 1.4f);
            }
        }
    }
}
