using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Player;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable
{
    public interface IPickable
    {
        void GetThrown(Vector2 pVelocity);
        void GetDropped(Vector2 pVelocity);
        void GetPickedUp(PlayerController2D pPicker);
        Transform Transform { get; }
    }
}
