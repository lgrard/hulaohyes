using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public class PlayerMovement : PlayerComponent
    {
        private float currentAcceleration;

        private void OnEnable() => player.inputHandler.controlScheme.Player.Movement.performed += ChangeDirection;
        private void OnDisable() => player.inputHandler.controlScheme.Player.Movement.performed -= ChangeDirection;
        private void FixedUpdate()
        {
            if (player.inputHandler.moveInput.magnitude < 0.1f && Mathf.Abs(currentAcceleration) > 0.1f)
                Brake();

            else if (player.inputHandler.moveInput.magnitude > 0.1f)
                Accelerate();

            Vector2 lVelocity = new Vector2(currentAcceleration, player.rb.velocity.y);
            player.rb.velocity = lVelocity;
        }

        private void Brake()
        {
            currentAcceleration -= (Mathf.Sign(currentAcceleration) * Time.deltaTime * player.playerDataSet.movementBrake);
        }

        private void Accelerate()
        {
            float lControlRatio = player.isGrounded ? 1 : player.playerDataSet.airControl;
            float lAcceleration = player.inputHandler.moveInput.x * player.playerDataSet.movementAcceleration * Time.deltaTime * lControlRatio;

            currentAcceleration = Mathf.Clamp(
                currentAcceleration + lAcceleration,
                -player.playerDataSet.movementSpeed,
                player.playerDataSet.movementSpeed);
        }

        private void ChangeDirection(InputAction.CallbackContext ctx)
        {
            int lDirection = ctx.ReadValue<Vector2>().x > 0 ? 1:-1;
            player?.onChangeDirection.Invoke(lDirection);
        }
    }
}