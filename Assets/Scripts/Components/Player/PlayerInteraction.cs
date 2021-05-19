using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using hulaohyes.Assets.Scripts.Targettable;
using hulaohyes.Assets.Scripts.Targettable.Pickable;
using hulaohyes.Assets.Scripts.Targettable.Interactables;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public class PlayerInteraction : PlayerComponent
    {
        [SerializeField] private Transform pickUpPoint;
        private IPickable currentPickUpTarget;
        private Interactable currentInteractTarget;

        private void OnEnable()
        {
            currentPickUpTarget = null;
            currentInteractTarget = null;
            player.onPickUpAquire += ChangePickUpTarget;
            player.onInteractAquire += ChangeInteractTarget;
            player.onTargetLoss += ResetTargets;
        }

        private void OnDisable()
        {
            currentPickUpTarget = null;
            currentInteractTarget = null;
            player.onPickUpAquire -= ChangePickUpTarget;
            player.onInteractAquire -= ChangeInteractTarget;
            player.onTargetLoss -= ResetTargets;
        }

        void Interact(InputAction.CallbackContext ctx)
        {
            if(currentInteractTarget != null)
            {
                currentInteractTarget.onInteract?.Invoke();
                player.onInteract?.Invoke();
                player.transform.position = new Vector2(currentInteractTarget.transform.position.x, currentInteractTarget.transform.position.y);
            }
        }

        void PickUp(InputAction.CallbackContext ctx)
        {
            if(currentPickUpTarget != null)
            {
                player.inputHandler.controlScheme.Player.PickUp.performed -= PickUp;
                player.inputHandler.controlScheme.Player.PickUp.performed += Throw;
                player.inputHandler.controlScheme.Player.Drop.performed += Drop;

                currentPickUpTarget.GetPickedUp(player);
                player.onPickUp?.Invoke();
                currentPickUpTarget.Transform.parent = pickUpPoint;
                currentPickUpTarget.Transform.localPosition = Vector3.zero;
            }
        }

        void Throw(InputAction.CallbackContext ctx)
        {
            player.inputHandler.controlScheme.Player.Drop.performed -= Drop;
            player.inputHandler.controlScheme.Player.PickUp.performed -= Throw;

            currentPickUpTarget?.GetThrown(player.direction * player.playerDataSet.throwForce);
            player.onThrow?.Invoke();
        }

        void Drop(InputAction.CallbackContext ctx)
        {
            player.inputHandler.controlScheme.Player.Drop.performed -= Drop;
            player.inputHandler.controlScheme.Player.PickUp.performed -= Throw;

            currentPickUpTarget?.GetDropped(player.direction * player.playerDataSet.dropForce);
            player.onDrop?.Invoke();
        }

        void ChangePickUpTarget(IPickable pTarget)
        {
            player.inputHandler.controlScheme.Player.PickUp.performed -= Interact;
            player.inputHandler.controlScheme.Player.PickUp.performed += PickUp;
            currentPickUpTarget = pTarget;
        }

        void ChangeInteractTarget(Interactable pTarget)
        {
            player.inputHandler.controlScheme.Player.PickUp.performed -= PickUp;
            player.inputHandler.controlScheme.Player.PickUp.performed += Interact;
            currentInteractTarget = pTarget;
        }

        private void ResetTargets()
        {
            currentPickUpTarget = null;
            currentInteractTarget = null;

            player.inputHandler.controlScheme.Player.Drop.performed -= Drop;
            player.inputHandler.controlScheme.Player.PickUp.performed -= PickUp;
            player.inputHandler.controlScheme.Player.PickUp.performed -= Interact;
        }
    }
}