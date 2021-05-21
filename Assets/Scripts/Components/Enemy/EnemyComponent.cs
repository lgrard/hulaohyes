using UnityEngine;
using System.Collections;
using hulaohyes.Assets.Scripts.Targettable.Pickable.Enemy;

namespace hulaohyes.Assets.Scripts.Components.Enemy
{
    public class EnemyComponent : MonoBehaviour
    {
        [HideInInspector]
        public EnemyController2D enemy;
    }
}