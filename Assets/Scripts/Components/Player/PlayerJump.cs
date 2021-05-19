using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public class PlayerJump : PlayerComponent
    {
        private void OnEnable() => player.inputHandler.controlScheme.Player.Jump.performed += Jump;
        private void OnDisable() => player.inputHandler.controlScheme.Player.Jump.performed -= Jump;

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (player.isGrounded)
            {
                player.rb.velocity = new Vector2(player.rb.velocity.x, player.playerDataSet.jumpHeight);
                player.onJump?.Invoke();
            }
        }
    }
}