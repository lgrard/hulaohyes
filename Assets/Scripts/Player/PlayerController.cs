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
        [Tooltip("The transform where pick up target is stored")]
        [SerializeField] Transform _pickUpPoint;

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

        public ControlScheme getActiveControlScheme() { return _controlScheme; }

        override protected void Init()
        {
            base.Init();
            _cameraContainer = CameraManager.getInstance().GetCamera(playerIndex).transform;
            _playerAnimator = GetComponent<Animator>();
            _controlScheme = new ControlScheme();
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions = _controlScheme.asset;
            SetPlayerDevice();
            _stateMachine = new PlayerStateMachine(this, _controlScheme, _cameraContainer, _rb, _playerAnimator, _attackPoint,_pickUpPoint, _playerParticles);

            _hp = maxHp;
        }

        override public void Propel()
        {
            CurrentPicker.SelfDropTarget(false);
            base.Propel();
            _stateMachine.CurrentState = _stateMachine.thrown;
        }

        override protected private void GetPicked()
        {
            base.GetPicked();
            _stateMachine.CurrentState = _stateMachine.carried;
        }

        private protected override void GetDropped()
        {
            base.GetDropped();
            _stateMachine.CurrentState = _stateMachine.running;
        }

        public void SelfDropTarget(bool pDrop)
        {
            if (_stateMachine.CurrentState == _stateMachine.carrying) StartCoroutine(_stateMachine.carrying.DropTarget(pDrop));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(_stateMachine.CurrentState == _stateMachine.thrown) _stateMachine.thrown.HitObject(collision);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(_attackPoint.localPosition, new Vector3(1,2,1));
        }

        ///Destroy player's object and delete references
        public void destroyPlayer()
        {
            Destroy(gameObject);
        }
    }
}