using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.door
{
    public class DoorManager : MonoBehaviour
    {
        [Range(1, 10)] [SerializeField] int _unitRequirement = 1;
        private int _currentUnits = 0;

        [Header("Associated door")]
        [SerializeField] GameObject _door;
        [SerializeField] AnimationCurve _openningCurve;

        [Header("Associated slabs")]
        [SerializeField] List<Slab> _slabList;

        [Header("Associated Color")]
        [SerializeField] Color _color;
        [SerializeField] bool _drawGizmos = true;

        private void Start()
        {
            foreach(Slab lSlab in _slabList)
            {
                if(lSlab != null)
                {
                    lSlab.Color = _color;
                    lSlab.DoorManager = this;
                }
            }
        }

        public IEnumerator OpenDoor()
        {
            foreach (Slab lSlab in _slabList)
                if(lSlab!=null) lSlab.enabled = false;

            Vector3 lRotationAmount = new Vector3(0, 1, 0);
            while(_door.transform.localEulerAngles.y < 90)
            {
                _door.transform.Rotate(lRotationAmount);
                yield return new WaitForEndOfFrame();
            }

            this.enabled = false;
        }

        public void SetUnit(int pAmount)
        {
            _currentUnits += pAmount;
            if (_currentUnits >= _unitRequirement) StartCoroutine(OpenDoor());
        }

        public List<Slab> SlabList { get => _slabList; }
        public GameObject Door { get => _door; }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(this.transform.position, 0.25f);

                Gizmos.color = _color;
                if(_door != null)
                {
                    Gizmos.DrawSphere(_door.transform.position, 0.15f);
                    foreach (Slab lSlab in _slabList)
                    {
                        if(lSlab != null)
                        {
                            Gizmos.DrawLine(_door.transform.position, lSlab.transform.position);
                            Gizmos.DrawSphere(lSlab.transform.position, 0.15f);
                        }
                    }
                }
            }
        }
    }
}
