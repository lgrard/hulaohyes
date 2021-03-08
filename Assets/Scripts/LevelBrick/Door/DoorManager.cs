﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.door
{
    public class DoorManager : MonoBehaviour
    {
        private const float DOOR_DURATION = 0.5f;

        [Range(1, 10)] [SerializeField] int unitRequirement = 1;
        private int currentUnits = 0;

        [Header("Associated door")]
        [SerializeField] List<GameObject> doorList;
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

        private void Start()
        {
            doorPositions = new List<float>();
            foreach (GameObject lDoor in doorList) doorPositions.Add(lDoor.transform.position.y);

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
            doorIsOpening = (currentUnits >= unitRequirement) ? true : false;
        }

        /// Returns this door manager's slab list
        public List<Slab> SlabList { get => slabList; }
        /// Returns this door manager's door list
        public List<GameObject> DoorList { get => doorList; }

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
