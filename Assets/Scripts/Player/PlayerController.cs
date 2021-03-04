using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hulaohyes.player.states;
using hulaohyes.inputs;
using hulaohyes.camera;
using hulaohyes.enemy;
using hulaohyes.effects;

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
        [Tooltip("The transform where pick up target is stored")]
        public Transform pickUpPoint;

        [Header("Particles list")]
        [SerializeField] List<ParticleSystem> _playerParticles;

        [HideInInspector]
        public Pickable pickUpTarget;

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
            if (_hp >= 0 && _stateMachine.CurrentState == _stateMachine.Carrying)
                _stateMachine.Carrying.TakeCarryDamage();

            else if (_hp >= 0 && _stateMachine.CurrentState != _stateMachine.Carrying && _stateMachine.CurrentState != _stateMachine.Carried)
            {
                _playerAnimator.SetTrigger("TakeDamage");
                _stateMachine.CurrentState = _stateMachine.Wait;
            }

            else if (_hp <= 0)
                _stateMachine.CurrentState = _stateMachine.Downed;
        }

        public void SetPlayerDevice()
        {
            InputDevice lDevice = DeviceManager.GetInputDevice(playerIndex);
            if (lDevice != null) _playerInput.SwitchCurrentControlScheme(lDevice);
        }
        public void DropTarget() => _stateMachine.CurrentState = _stateMachine.Wait;

        override protected void Init()
        {
            base.Init();
            _cameraContainer = CameraManager.getInstance().GetCamera(playerIndex).transform;
            _playerAnimator = GetComponent<Animator>();
            _controlScheme = new ControlScheme();
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions = _controlScheme.asset;
            SetPlayerDevice();
            _stateMachine = new PlayerStateMachine(this, _controlScheme, _cameraContainer, _rb, _playerAnimator, _playerParticles, _collider);

            _hp = maxHp;
        }

        override public void Propel()
        {
            _stateMachine.CurrentState = _stateMachine.Thrown;
            base.Propel();
        }

        override public void Drop()
        {
            base.Drop();
            _stateMachine.CurrentState = _stateMachine.Thrown;
        }

        override public void GetPicked(PlayerController pPicker)
        {
            base.GetPicked(pPicker);
            if (pickUpTarget != null) DropTarget();
            _stateMachine.CurrentState = _stateMachine.Carried;
        }

        protected override void HitSomething()
        {
            if (isThrown)
            {
                base.HitSomething();
                _stateMachine.CurrentState = _stateMachine.Running;
                _playerParticles[2].Play();
            }
        }

        protected override void HitEnemy(EnemyController pEnemy)
        {
            base.HitEnemy(pEnemy);
            Effects.HitStop(_playerAnimator, pEnemy.EnemyAnimator, 0.5f, 0.1f);
        }

        bool isThrown => _stateMachine.CurrentState == _stateMachine.Thrown;
        float _gravity => (_rb.velocity.y < 0) ? GRAVITY_AMOUNT_FALL : GRAVITY_AMOUNT_RISE;
        public ControlScheme getActiveControlScheme() { return _controlScheme; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
        }

        ///Destroy player's object and delete references
        public void destroyPlayer()
        {
            Destroy(gameObject);
        }
    }
}