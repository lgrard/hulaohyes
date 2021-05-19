using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private ControlScheme _controlScheme= null;
        private PlayerInput playerInput = null;

        private void Awake()
        {
            _controlScheme = new ControlScheme();
            _controlScheme.Enable();
            if (TryGetComponent<PlayerInput>(out PlayerInput pPlayerInput)) playerInput = pPlayerInput;
            else gameObject.AddComponent<PlayerInput>();
            playerInput.actions = controlScheme.asset;
        }

        private void OnEnable()
        {
            _controlScheme.Enable();
        }

        private void OnDisable()
        {
            _controlScheme.Disable();
        }

        public ControlScheme controlScheme => _controlScheme;
        public Vector2 moveInput => _controlScheme.Player.Movement.ReadValue<Vector2>();
    }
}