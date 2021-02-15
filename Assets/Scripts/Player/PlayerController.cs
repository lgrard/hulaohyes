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

        public int playerIndex = 0;

        [Header("HP values")]
        private int maxHp = 10;
        private int _hp;

        [Header("Objects and components")]
        [Tooltip("Main camera container")]
        [SerializeField] Transform _cameraContainer;
        [SerializeField] Transform _attackPoint;

        [Header("Particles list")]
        [SerializeField] List<ParticleSystem> _playerParticles;

        [Header("Current pick up target")]
        public Pickable pickUpTarget;

        ///Constructor and initialization
        private void Start() => Init();

        ///Standard and physic GameLoops
        private void Update() => _stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            _stateMachine.CurrentState.PhysLoopLogic();
            _rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);
        }

        /// The player takes a certain amount of damage
        /// <param name="pDamage"> Amount of damage taken </param>
        public void TakeDamage(int pDamage)
        {
            _hp -= pDamage;
            if (_hp >= 0 && _stateMachine.CurrentState == _stateMachine.carrying)
                _stateMachine.carrying.TakeCarryDamage();

            else if (_hp >= 0 && _stateMachine.CurrentState != _stateMachine.carrying && _stateMachine.CurrentState != _stateMachine.carried)
                _stateMachine.CurrentState = _stateMachine.takingDamage;

            else if (_hp <= 0)
                _stateMachine.CurrentState = _stateMachine.downed;
        }

        public void PunchHit() => _stateMachine.attacking.PunchHitTest();

        float _gravity => (_rb.velocity.y < 0) ? GRAVITY_AMOUNT_FALL : GRAVITY_AMOUNT_RISE;

        override protected void Init()
        {
            base.Init();
            _playerInput = GetComponent<PlayerInput>();
            _playerAnimator = GetComponent<Animator>();
            _controlScheme = new ControlScheme();
            _playerInput.actions = _controlScheme.asset;
            _stateMachine = new PlayerStateMachine(this, _controlScheme, _cameraContainer, _rb, _playerAnimator, _attackPoint, _playerParticles);

            _hp = maxHp;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
            Gizmos.DrawWireSphere(_attackPoint.position, 1f);
        }

        ///Destroy player's object and delete references
        public void destroyPlayer()
        {
            //Destructor
        }
    }
}