using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hulaohyes.Assets.Scripts.Targettable;
using hulaohyes.Assets.Scripts.Targettable.Interactables;
using hulaohyes.Assets.Scripts.Targettable.Pickable;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public class PlayerTargetting : PlayerComponent
    {
        private IPickable currentPickUpTarget = null;
        private Interactable currentInteractTarget = null;

        [Header("Debug")]
        [SerializeField] bool drawGizmos;

        private void FixedUpdate() => Targetting();

        void Targetting()
        {
            RaycastHit2D[] lHits = Physics2D.RaycastAll(transform.position, player.direction, player.playerDataSet.pickUpDistance);
            IPickable lNewPickUpTarget = null;
            Interactable lNewInteractableTarget = null;

            foreach (RaycastHit2D lHit in lHits)
            {
                if(lHit.collider.gameObject.TryGetComponent<ITargettable>(out ITargettable lTarget))
                {
                    if (lTarget == currentInteractTarget as ITargettable || lTarget == currentPickUpTarget as ITargettable)
                        return;

                    else if (lTarget != player as ITargettable && lTarget.isTargettable)
                    {
                        player?.onTargetAquire.Invoke();

                        if (lHit.collider.gameObject.TryGetComponent<Interactable>(out Interactable lInteractTarget))
                        {
                            player?.onInteractAquire?.Invoke(lInteractTarget);
                            lNewInteractableTarget = lInteractTarget;
                        }

                        else if (lHit.collider.gameObject.TryGetComponent<IPickable>(out IPickable lPickUpTarget))
                        {
                            player?.onPickUpAquire?.Invoke(lPickUpTarget);
                            lNewPickUpTarget = lPickUpTarget;
                        }
                    }
                }
            }

            if ((lNewPickUpTarget == null && currentPickUpTarget != null)
                || (lNewInteractableTarget == null && currentInteractTarget != null))
                player?.onTargetLoss.Invoke();

            currentInteractTarget = lNewInteractableTarget;
            currentPickUpTarget = lNewPickUpTarget;
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos && player != null)
            {
                Gizmos.DrawLine(transform.position, transform.position + player.direction * player.playerDataSet.pickUpDistance);
            }
        }
    }
}