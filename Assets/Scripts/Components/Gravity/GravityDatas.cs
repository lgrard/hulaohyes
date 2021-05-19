using UnityEngine;

namespace hulaohyes.Assets.Scripts.Components.Gravity
{
    [CreateAssetMenu(fileName ="GravityData_0",menuName ="HulaOhYes/Data/Gravity",order = 0)]
    public class GravityDatas : ScriptableObject
    {
        [SerializeField] private float _risingGravity;
        [SerializeField] private float _fallingGravity;

        //Getters
        public float risingGravity => _risingGravity;
        public float fallingGravity => _fallingGravity;
    }
}