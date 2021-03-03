using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.door
{
    public class DoorManager : MonoBehaviour
    {
        private const float DOOR_DURATION = 0.5f;

        [Range(1, 10)] [SerializeField] int _unitRequirement = 1;
        private int _currentUnits = 0;

        [Header("Associated door")]
        [SerializeField] List<GameObject> _doorList;
        [SerializeField] AnimationCurve _doorCurve;
        [Range(0,10)][SerializeField] float _doorHeight = 3.1f;
        private List<float> _doorPositions;
        private float _doorProgress;
        private bool _doorIsOpening;

        [Header("Associated slabs")]
        [SerializeField] List<Slab> _slabList;

        [Header("Associated Color")]
        [SerializeField] Color _color;

        [Tooltip("Are the gizmos drawing fo this object")]
        [SerializeField] bool _drawGizmos = true;

        private void Start()
        {
            _doorPositions = new List<float>();
            foreach (GameObject lDoor in _doorList) _doorPositions.Add(lDoor.transform.position.y);

            foreach (Slab lSlab in _slabList)
            {
                if(lSlab != null)
                {
                    lSlab.Color = _color;
                    lSlab.DoorManager = this;
                }
            }
        }

        private void Update()
        {
            if (_doorIsOpening && _doorProgress < DOOR_DURATION)
            {
                _doorProgress += Time.deltaTime;
                MoveDoor();
            }

            else if (!_doorIsOpening && _doorProgress > 0)
            {
                _doorProgress -= Time.deltaTime;
                MoveDoor();
            }
        }

        private void MoveDoor()
        {
            for (int i = _doorList.Count - 1; i >= 0; i--)
            {
                float lDoorY = Mathf.Lerp(_doorPositions[i], _doorPositions[i]- _doorHeight, _doorCurve.Evaluate(_doorProgress / DOOR_DURATION));
                _doorList[i].transform.position = new Vector3(_doorList[i].transform.position.x, lDoorY, _doorList[i].transform.position.z);
            }
        }

        /// Add/Remove a specified amount of unit to this door manager
        /// <param name="pAmount"> Amount of unit to increase/decrease </param>
        public void SetUnit(int pAmount)
        {
            _currentUnits += pAmount;
            _doorIsOpening = (_currentUnits >= _unitRequirement) ? true : false;
        }

        /// Returns this door manager's slab list
        public List<Slab> SlabList { get => _slabList; }
        /// Returns this door manager's door list
        public List<GameObject> DoorList { get => _doorList; }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(this.transform.position, 0.25f);

                Gizmos.color = _color;
                if(_doorList != null)
                {
                    foreach (GameObject lDoor in _doorList)
                    {
                        if (lDoor != null)
                        {
                            Gizmos.DrawSphere(lDoor.transform.position, 0.15f);

                            foreach (Slab lSlab in _slabList)
                            {
                                if (lSlab != null)
                                {
                                    Gizmos.DrawLine(lDoor.transform.position, lSlab.transform.position);
                                    Gizmos.DrawSphere(lSlab.transform.position, 0.15f);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
