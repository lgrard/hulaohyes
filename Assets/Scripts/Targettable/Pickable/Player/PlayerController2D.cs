using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.Assets.Scripts.Collision;
using hulaohyes.Assets.Scripts.Components.Player;
using hulaohyes.Assets.Scripts.Targettable.Interactables;
using hulaohyes.Assets.Scripts.Timers;

namespace hulaohyes.Assets.Scripts.Targettable.Pickable.Player
{
    public class PlayerController2D : MonoBehaviour, IPickable, ITargettable
    {
        public delegate void EventHandler();
        public EventHandler onJump = null;
        public EventHandler onLand = null;
        public EventHandler onPickUp = null;
        public EventHandler onThrow = null;
        public EventHandler onDrop = null;
        public EventHandler onGetPickedUp = null;
        public EventHandler onGetThrown = null;
        public EventHandler onGetDropped = null;
        public EventHandler onTargetLoss = null;
        public EventHandler onTargetAquire = null;
        public EventHandler onInteract = null;

        public delegate void TypedEventHandler<T>(T pValue);
        public TypedEventHandler<IPickable> onPickUpAquire = null;
        public TypedEventHandler<Interactable> onInteractAquire = null;
        public TypedEventHandler<int> onChangeDirection = null;

        private Rigidbody2D _rb;
        private bool _isGrounded = true;
        private Vector2 _direction = new Vector2(1, 0);

        [SerializeField] int playerIndex = 0;
        [SerializeField] PlayerDatas _playerDataSet = null;
        [SerializeField] LayerMask groundLayer;
        
        [Header("Components")]
        [SerializeField] CollisionHandler collisionHandler = null;
        [SerializeField] PlayerInputHandler _inputHandler = null;
        [SerializeField] PlayerTargetting targetting = null;

        private void Awake()
        {
            foreach (PlayerComponent lPlayerComponent in GetComponentsInChildren<PlayerComponent>())
                lPlayerComponent.player = this;
        }

        private void Start() => Init();

        private void FixedUpdate()
        {
            _isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, _playerDataSet.groundCheckDistance, groundLayer);
            _direction = UpdateDirection();
        }

        Vector2 UpdateDirection()
        {
            if (_inputHandler.moveInput.x != 0)
                return new Vector2(_inputHandler.moveInput.x, 0).normalized;

            return _direction;
        }

        private void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            onPickUp += OnPickUpTarget;
            onThrow += ClearState;
            onDrop += ClearState;
            onInteract += OnInteract;
        }

        public void GetThrown(Vector2 pVelocity)
        {
            transform.parent = null;
            rb.isKinematic = false;
            onGetThrown?.Invoke();
        }

        public void GetDropped(Vector2 pVelocity)
        {
            transform.parent = null;
            rb.isKinematic = false;
            onGetDropped?.Invoke();
        }

        public void GetPickedUp(PlayerController2D pCaster)
        {
            onGetPickedUp?.Invoke();
        }


        //State component logic
        private void ClearState()
        {
            targetting.enabled = true;
            _inputHandler.enabled = true;
            rb.isKinematic = false;
        }
        private void OnPickUpTarget() => targetting.enabled = false;
        private void OnInteract()
        {
            targetting.enabled = false;
            _inputHandler.enabled = false;
            rb.isKinematic = true;

            new Timer(0.5f).onTimerEnd = ClearState;
        }


        public bool isTargettable => true;
        public Transform Transform => transform;


        //Getters
        public PlayerDatas playerDataSet => _playerDataSet;
        public Rigidbody2D rb => _rb;
        public PlayerInputHandler inputHandler => _inputHandler;
        public bool isGrounded => _isGrounded;
        public Vector3 direction => _direction;

        //Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            if(playerDataSet != null)
                Gizmos.DrawLine(Vector3.zero, -Vector3.up * playerDataSet.groundCheckDistance);
        }
    }
}
