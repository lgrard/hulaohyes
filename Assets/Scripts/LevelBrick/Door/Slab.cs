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
        private PlayerController _currentPlayer;
        private int _currentUnits;
        private LayerMask _collisionLayer;

        [Range(1,2)][SerializeField] int _maxUnit = 2;
        [Tooltip("Where will the unit cube will magnet")]
        [SerializeField] Vector3 _cubeSlotPosition;
        [Tooltip("TriggerSize")]
        [SerializeField] Vector3 _triggerZoneSize;
        [Tooltip("Are the gizmos drawing fo this object")]
        [SerializeField] bool _drawGizmos;

        void Start()
        {
            _collisionLayer = LayerMask.GetMask("Player", "Bricks");
            CreateTrigger();
        }

        void CreateTrigger()
        {
            BoxCollider lTrigger = gameObject.AddComponent<BoxCollider>();
            lTrigger.size = _triggerZoneSize;
            lTrigger.center = new Vector3(0, _triggerZoneSize.y / 2, 0);
            lTrigger.isTrigger = true;
        }

        void MagnetCube(UnitCube pCube)
        {
            _currentUnitCube = pCube;
            _currentUnitCube.transform.parent = this.transform;
            _currentUnitCube.transform.localPosition = _cubeSlotPosition;
            _currentUnitCube.transform.eulerAngles = Vector3.zero;
            _currentUnitCube.FreezeCube(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_collisionLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                if (_currentUnits >= _maxUnit) return;

                if(other.TryGetComponent<UnitCube>(out UnitCube pCube))
                {
                    if (_currentUnitCube == null) MagnetCube(pCube);
                    else return;
                }

                else if (other.TryGetComponent<PlayerController>(out PlayerController pPlayer))
                {
                    if (_currentPlayer == null) _currentPlayer = pPlayer;
                    else return;
                }

                _currentUnits += 1;
                _doorManager.SetUnit(1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController pPlayer) && pPlayer == _currentPlayer)
            {
                _currentPlayer = null;
                _currentUnits -= 1;
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
                if (value == null) _currentUnits -= 1;
                _doorManager.SetUnit(-1);
                _currentUnitCube = value;
            }
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Vector3 lTriggerPosition = new Vector3(0, _triggerZoneSize.y / 2, 0);
                Gizmos.DrawWireCube(lTriggerPosition, _triggerZoneSize);
                Gizmos.color = Color.green;
                Vector3 lCubePosition = new Vector3(_cubeSlotPosition.x, _cubeSlotPosition.y+0.55f, _cubeSlotPosition.z);
                Gizmos.DrawWireCube(lCubePosition, Vector3.one * 1);
            }
        }
    }
}
