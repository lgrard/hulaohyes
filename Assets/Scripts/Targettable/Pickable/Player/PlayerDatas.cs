using UnityEngine;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable.Player
{
    [CreateAssetMenu(fileName = "PlayerData_0", menuName = "HulaOhYes/Data/Player", order = 0)]
    public class PlayerDatas : ScriptableObject
    {
        [Header("Logic values")]
        [SerializeField] private int _hp = 3;
        [SerializeField] private float _respawnTimer = 5;

        [Header("KnockBack values")]
        [SerializeField] private float _knockBackAmount = 1.5f;
        [SerializeField] private float _knockBackTime = 0.1f;

        [Header("Movements values")]
        [SerializeField] private float _movementSpeed = 6;
        [SerializeField] private float _movementAcceleration = 60;
        [SerializeField] private float _movementBrake = 30;
        [SerializeField] private float _jumpHeight = 8;
        [Range(0,1)][SerializeField] private float _airControl = 0.5f;
        [SerializeField] private float _groundCheckDistance = 0.5f;

        [Header("Pick up distance")]
        [SerializeField] private float _pickUpDistance = 2f;
        [SerializeField] private float _verticalProprelHeight = 8f;
        [SerializeField] private float _throwForce = 8f;
        [SerializeField] private float _dropForce = 2f;


        // Getters
        public int hp => _hp;
        public float respawnTimer => _respawnTimer;
        public float knockBackAmount => _knockBackAmount;
        public float knockBackTime => _knockBackTime;
        public float movementSpeed => _movementSpeed;
        public float jumpHeight => _jumpHeight;
        public float groundCheckDistance => _groundCheckDistance;
        public float pickUpDistance => _pickUpDistance;
        public float verticalProprelHeight => _verticalProprelHeight;
        public float throwForce => _throwForce;
        public float dropForce => _dropForce;
        public float airControl => _airControl;
        public float movementAcceleration => _movementAcceleration;
        public float movementBrake => _movementBrake;
    }
}
