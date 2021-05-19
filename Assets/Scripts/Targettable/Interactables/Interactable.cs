using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.Assets.Scripts.Actionables;

namespace hulaohyes.Assets.Scripts.Targettable.Interactables
{
    public class Interactable : MonoBehaviour, ITargettable
    {
        public delegate void Interaction();
        public Interaction onInteract;
        protected bool isInteractable = true;

        [SerializeField] protected List<Actionable> actionableObjectList;

        [Header("Debug")]
        [SerializeField] protected bool drawGizmos = true;

        private void OnEnable()
        {
            if (actionableObjectList != null)
            {
                foreach (Actionable lActionable in actionableObjectList)
                {
                    if (lActionable != null)
                    {
                        onInteract += lActionable.DoAction;
                    }
                }
            }
        }
        private void OnDisable()
        {
            if (actionableObjectList != null)
            {
                foreach (Actionable lActionable in actionableObjectList)
                {
                    if (lActionable != null)
                    {
                        onInteract -= lActionable.DoAction;
                    }
                }
            }
        }

        public bool isTargettable => isInteractable;

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position, 0.25f);

                if (actionableObjectList != null)
                {
                    foreach(Actionable lActionable in actionableObjectList)
                    {
                        if(lActionable != null)
                        {
                            Gizmos.DrawLine(transform.position, lActionable.transform.position);
                            Gizmos.DrawSphere(lActionable.transform.position, 0.1f);
                        }
                    }
                }
            }
        }
    }
}