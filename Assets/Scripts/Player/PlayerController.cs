using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hulaohyes.player.states;
using hulaohyes.inputs;
using hulaohyes.camera;

namespace hulaohyes.player
{
    public class PlayerController : Pickable
    {
        const float GRAVITY_AMOUNT_RISE = 2f;
        const float GRAVITY_AMOUNT_FALL = 4f;

        private GameManager _gameManager;
        private Animator _playerAnimator;
        private PlayerStateMachine _stateMachine;
        private PlayerInput _playerInput;
        private ControlScheme _controlScheme;
        private Transform _cameraContainer;

        public int playerIndex = 0;

        [Header("HP values")]
        private int maxHp = 10;
        private int _hp;

        [Header("Objects and components")]
        [Tooltip("The transform where the attack collision checks")]
        [SerializeField] Transform _attackPoint;

        [Header("Particles list")]
        [SerializeField] List<ParticleSystem> _playerParticles;

        [HideInInspector]
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
        public void ResetCombo() => _stateMachine.attacking.ResetCombo();

        float _gravity => (_rb.velocity.y < 0) ? GRAVITY_AMOUNT_FALL : GRAVITY_AMOUNT_RISE;

        public void SetPlayerDevice()
        {
            InputDevice lDevice = DeviceManager.GetInputDevice(playerIndex);
            if (lDevice != null) _playerInput.SwitchCurrentControlScheme(lDevice);
        }

        override protected void Init()
        {
            base.Init();
            _cameraContainer = CameraManager.getActiveCamera(this.transform);
            _playerAnimator = GetComponent<Animator>();
            _controlScheme = new ControlScheme();
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions = _controlScheme.asset;
            SetPlayerDevice();
            _stateMachine = new PlayerStateMachine(this, _controlScheme, _cameraContainer, _rb, _playerAnimator, _attackPoint, _playerParticles);

            _hp = maxHp;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
            Gizmos.DrawWireCube(_attackPoint.position, new Vector3(1,2,1));
        }

        ///Destroy player's object and delete references
        public void destroyPlayer()
        {
            Destroy(gameObject);
        }
    }
}