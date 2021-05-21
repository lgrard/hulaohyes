using System.Collections;
using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Player;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        [HideInInspector]
        public PlayerController2D player;
    }
}