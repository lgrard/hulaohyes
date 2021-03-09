using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.door
{
    public class DoorManager : MonoBehaviour
    {
        private const float DOOR_DURATION = 0.5f;

        [Range(1, 10)] [SerializeField] int _unitRequirement = 1;
        private int currentUnits = 0;

        [Header("Associated door")]
        [SerializeField] List<GameObject> doorList;
        [SerializeField] List<Door> doorCompList;
        [SerializeField] AnimationCurve doorCurve;
        [Range(0,10)][SerializeField] float doorHeight = 3.1f;
        private List<float> doorPositions;
        private float doorProgress;
        private bool doorIsOpening;

        [Header("Associated slabs")]
        [SerializeField] List<Slab> slabList;

        [Header("Associated Color")]
        [SerializeField] Color color;

        [Tooltip("Are the gizmos drawing fo this object")]
        [SerializeField] bool drawGizmos = true;

        private void Awake()
        {
            doorPositions = new List<float>();
            foreach (GameObject lDoor in doorList)
            {
                Door lDoorComponent = lDoor.GetComponentInParent<Door>();
                lDoorComponent.DoorManager = this;
                lDoorComponent.Color = color;
                doorCompList.Add(lDoorComponent);
                doorPositions.Add(lDoor.transform.position.y);
            }

            foreach (Slab lSlab in slabList)
            {
                if(lSlab != null)
                {
                    lSlab.Color = color;
                    lSlab.DoorManager = this;
                }
            }
        }

        private void Update()
        {
            if (doorIsOpening && doorProgress < DOOR_DURATION)
            {
                doorProgress += Time.deltaTime;
                MoveDoor();
            }

            else if (!doorIsOpening && doorProgress > 0)
            {
                doorProgress -= Time.deltaTime;
                MoveDoor();
            }
        }

        private void MoveDoor()
        {
            for (int i = doorList.Count - 1; i >= 0; i--)
            {
                float lDoorY = Mathf.Lerp(doorPositions[i], doorPositions[i]- doorHeight, doorCurve.Evaluate(doorProgress / DOOR_DURATION));
                doorList[i].transform.position = new Vector3(doorList[i].transform.position.x, lDoorY, doorList[i].transform.position.z);
            }
        }

        /// Add/Remove a specified amount of unit to this door manager
        /// <param name="pAmount"> Amount of unit to increase/decrease </param>
        public void SetUnit(int pAmount)
        {
            currentUnits += pAmount;
            foreach(Door lDoor in doorCompList)
            {
                if (currentUnits == 1 && _unitRequirement == 1) lDoor.DoorRenderer.materials[1].SetFloat("Brightness", 5);
                else if (currentUnits == 2 && _unitRequirement >= 2) lDoor.DoorRenderer.materials[2].SetFloat("Brightness", 5);
                else if (currentUnits == 1 && _unitRequirement >= 2)
                {
                    lDoor.DoorRenderer.materials[1].SetFloat("Brightness", 5);
                    lDoor.DoorRenderer.materials[2].SetFloat("Brightness", 0);
                }
                else lDoor.DoorRenderer.materials[1].SetFloat("Brightness", 0);
            }
            doorIsOpening = (currentUnits >= _unitRequirement) ? true : false;
        }

        /// Returns this door manager's slab list
        public List<Slab> SlabList { get => slabList; }
        /// Returns this door manager's door list
        public List<GameObject> DoorList { get => doorList; }

        public int UnitRequirement => _unitRequirement;

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(this.transform.position, 0.25f);

                Gizmos.color = color;
                if(doorList != null)
                {
                    foreach (GameObject lDoor in doorList)
                    {
                        if (lDoor != null)
                        {
                            Gizmos.DrawSphere(lDoor.transform.position, 0.15f);

                            foreach (Slab lSlab in slabList)
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
