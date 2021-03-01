using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;

namespace hulaohyes.levelbrick.door
{
    public class Slab : MonoBehaviour
    {
        private Color _color;
        private DoorManager _doorManager;
        private List<GameObject> _unitList;

        [SerializeField] Vector3 _triggerZoneSize;
        [SerializeField] bool _drawGizmos;

        void Start()
        {
            _unitList = new List<GameObject>();
            CreateTrigger();
        }

        void CreateTrigger()
        {
            BoxCollider lTrigger = gameObject.AddComponent<BoxCollider>();
            lTrigger.size = _triggerZoneSize;
            lTrigger.center = new Vector3(0, _triggerZoneSize.y / 2, 0);
            lTrigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController pPlayer) || other.TryGetComponent<Pickable>(out Pickable pCube))
            {
                _unitList.Add(other.gameObject);
                _doorManager.SetUnit(1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_unitList.Contains(other.gameObject))
            {
                _unitList.Remove(other.gameObject);
                _doorManager.SetUnit(-1);
            }
        }

        public Color Color { set => _color = value; }
        public DoorManager DoorManager { set => _doorManager = value; }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Vector3 lTriggerPosition = new Vector3(0, _triggerZoneSize.y / 2, 0);
                Gizmos.DrawWireCube(lTriggerPosition, _triggerZoneSize);
            }
        }
    }
}
