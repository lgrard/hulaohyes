using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.inputs
{
    public static class DeviceManager
    {
        private static InputDevice _inputDevice0;
        private static InputDevice _inputDevice1;

        /// Sets the players devices
        public static void SetInputDevice(InputAction.CallbackContext ctx)
        {
            InputDevice lDevice = ctx.control.device;

            if (_inputDevice0 == null && lDevice != _inputDevice1)
            {
                _inputDevice0 = lDevice;
                Debug.Log(_inputDevice0.name + " has been linked with player 0");
            }
            else if (_inputDevice1 == null && lDevice != _inputDevice0)
            {
                _inputDevice1 = lDevice;
                Debug.Log(_inputDevice1.name + " has been linked with player 1");
            }
            else if (_inputDevice0 != null && _inputDevice1 != null) Debug.Log("Both player paired");
        }

        public static void WithdrawInputDevice(InputAction.CallbackContext ctx)
        {
            if (ctx.control.device == _inputDevice0) _inputDevice0 = null;
            else if (ctx.control.device == _inputDevice1) _inputDevice1 = null;
        }

        /// Returns an associated player device
        /// <param name="pPlayerIndex"> Associated player index </param>
        /// <returns> Returns a player device </returns>
        public static InputDevice GetInputDevice(int pPlayerIndex)
        {
            if (pPlayerIndex == 0) return _inputDevice0;
            else if (pPlayerIndex == 1) return _inputDevice1;
            else
            {
                Debug.LogError("Invalid player index");
                return null; 
            }
        }
    }
}
