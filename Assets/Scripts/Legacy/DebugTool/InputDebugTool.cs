using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace hulaohyes.debugtool
{
    public class InputDebugTool : MonoBehaviour
    {
        static InputDebugTool _instance;

        [Header("Input devices (Debug Only)")]
        [SerializeField] string p0_input_debug;
        [SerializeField] string p1_input_debug;

        private PlayerInput player0input;
        private PlayerInput player1input;
        private ControlScheme player0cs;
        private ControlScheme player1cs;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(this);
        }

        private void OnEnable() => StartCoroutine(Init());

        private void OnDisable()
        {
            if (player0cs != null)
            {
                player0cs.Player.Debug_Switch.performed -= SwitchControllers;
                player0cs.Player.Debug_Reload.performed -= ReloadScene;
            }

            if (player1cs != null)
            {
                player1cs.Player.Debug_Switch.performed -= SwitchControllers;
                player1cs.Player.Debug_Reload.performed -= ReloadScene;
            }
        }

        IEnumerator Init()
        {
            yield return new WaitForEndOfFrame();
            PlayerController lPlayer0 = GameManager.getPlayer(0);
            PlayerController lPlayer1 = GameManager.getPlayer(1);
            if (lPlayer0 != null && lPlayer1 != null)
            {
                player0input = lPlayer0.gameObject.GetComponent<PlayerInput>();
                player1input = lPlayer1.gameObject.GetComponent<PlayerInput>();

                if (player0input.devices.Count > 0 && player1input.devices.Count > 0)
                {
                    player0cs = lPlayer0.getActiveControlScheme();
                    player1cs = lPlayer1.getActiveControlScheme();
                    player0cs.Player.Debug_Switch.performed += SwitchControllers;
                    player1cs.Player.Debug_Switch.performed += SwitchControllers;
                    player0cs.Player.Debug_Reload.performed += ReloadScene;
                    player1cs.Player.Debug_Reload.performed += ReloadScene;

                    p0_input_debug = player0input.devices[0].name;
                    p1_input_debug = player1input.devices[0].name;
                }

                else Disable();
            }

            else Disable();
        }

        void ReloadScene(InputAction.CallbackContext ctx)=> SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        void SwitchControllers(InputAction.CallbackContext ctx)
        {
            InputDevice lPlayer0Device = player0input.devices[0];
            InputDevice lPlayer1Device = player1input.devices[0];

            if (lPlayer0Device != null && lPlayer1Device != null)
            {
                player0input.SwitchCurrentControlScheme(lPlayer1Device);
                player1input.SwitchCurrentControlScheme(lPlayer0Device);
                Debug.Log("Switch: device switch between players");

                p0_input_debug = lPlayer1Device.name;
                p1_input_debug = lPlayer0Device.name;
            }
        }

        void Disable()
        {
            Debug.Log("player1 device not detected, disabling InputDebugTool");
            this.enabled = false;
        }
    }
}
