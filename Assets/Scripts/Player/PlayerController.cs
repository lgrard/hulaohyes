using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hulaohyes.player.states;

namespace hulaohyes.player
{
    public class PlayerController : Pickable
    {
        const float GRAVITY_AMOUNT_RISE = 2f;
        const float GRAVITY_AMOUNT_FALL = 4f;

        private Animator _playerAnimator;
        private PlayerStateMachine _stateMachine;
        private PlayerInput _playerInput;
        private ControlScheme _controlScheme;

        [Tooltip("Main camera container")]
        [SerializeField] Transform _cameraContainer;
        [SerializeField] Transform _attackPoint;

        [Header("Current pick up target")]
        public Pickable pickUpTarget;

        //Constructor and initialization
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();
            _playerAnimator = GetComponent<Animator>();
            _controlScheme = new ControlScheme();
            _playerInput.actions = _controlScheme.asset;
            _stateMachine = new PlayerStateMachine(this,_controlScheme,_cameraContainer,_rb,_playerAnimator, _attackPoint);
        }

        private void Update() => _stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            _stateMachine.CurrentState.PhysLoopLogic();
            _rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);
        }

        float _gravity => (_rb.velocity.y < 0) ? GRAVITY_AMOUNT_FALL : GRAVITY_AMOUNT_RISE;

        private void OnTriggerEnter(Collider other)
        {

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
            Gizmos.DrawWireSphere(_attackPoint.position, 1f);
        }

        ///Destroy player's object and delete references
        public PlayerController destroyPlayer()
        {
            //Destructor
            return this;
        }
    }
}