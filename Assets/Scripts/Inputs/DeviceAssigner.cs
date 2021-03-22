using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace hulaohyes.inputs
{
    public class DeviceAssigner : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private ControlScheme _controlScheme;

        [SerializeField] List<GameObject> indicatorList;
        [SerializeField] UnityEvent doWhenPaired;

        private void OnEnable()
        {
            DeviceManager.ClearInputDevices();
            for (int i=0; i<=1; i++) indicatorList[i].SetActive(true);

            if (_controlScheme == null)
            {
                _controlScheme = new ControlScheme();
                _playerInput = GetComponent<PlayerInput>();
                _playerInput.actions = _controlScheme.asset;
            }

            _controlScheme.Enable();
            _controlScheme.Player.PickUp.performed += PairDevice;
            _controlScheme.Player.Drop.performed += UnPairDevice;
        }

        private void OnDisable()
        {
            _controlScheme.Disable();
            _controlScheme.Player.PickUp.performed -= PairDevice;
            _controlScheme.Player.Drop.performed -= UnPairDevice;
        }

        void PairDevice(InputAction.CallbackContext ctx)
        {
            DeviceManager.SetInputDevice(ctx);
            for (int i = 0; i <= 1; i++) indicatorList[i].SetActive(DeviceManager.GetInputDevice(i) == null);
            if (DeviceManager.bothPaired) doWhenPaired.Invoke();
        }

        void UnPairDevice(InputAction.CallbackContext ctx)
        {
            DeviceManager.WithdrawInputDevice(ctx);
            for (int i = 0; i <= 1; i++) indicatorList[i].SetActive(DeviceManager.GetInputDevice(i) == null);
        }
    }
}
