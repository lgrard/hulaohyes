using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.inputs
{
    public class DeviceAssigner : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private ControlScheme _controlScheme;

        private void OnEnable()
        {
            _controlScheme = new ControlScheme();
            _controlScheme.Enable();
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions = _controlScheme.asset;
            _controlScheme.Player.Join.performed += DeviceManager.SetInputDevice;
        }

        private void OnDisable()
        {
            _controlScheme.Disable();
            _controlScheme.Player.Join.performed -= DeviceManager.SetInputDevice;
        }
    }
}
