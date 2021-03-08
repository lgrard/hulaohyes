﻿using System.Collections;
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
        const float KNOCK_BACK_AMOUNT = 0.3f;
        const float KNOCK_BACK_TIME = 0.1f;

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

        [Header("Player values")]
        public float MOVEMENT_SPEED = 6;                                                                                                //const to change
        public float JUMP_HEIGHT = 8;                                                                                                   //const to change
        public float GROUND_CHECK_DISTANCE = 0.5f;
        public float PICK_UP_DISTANCE =2f;                                                                                              //const to change
        public float VERTICAL_PROPEL_HEIGHT = 8f;                                                                                       //const to change

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
        public void TakeDamage(int pDamage, Transform pDealer)
        {
            _hp -= pDamage;
            if (_hp >= 0 && _stateMachine.CurrentState == _stateMachine.Carrying)
                _stateMachine.Carrying.TakeCarryDamage();

            else if (_hp >= 0 && _stateMachine.CurrentState != _stateMachine.Carrying && _stateMachine.CurrentState != _stateMachine.Carried)
            {
                StartCoroutine(KnockBack(pDealer));
                _playerAnimator.SetTrigger("TakeDamage");
                _stateMachine.CurrentState = _stateMachine.Wait;
            }

            else if (_hp <= 0)
                _stateMachine.CurrentState = _stateMachine.Downed;
        }

        private IEnumerator KnockBack(Transform pOrigin)
        {
            float lTimeStamp = KNOCK_BACK_TIME;
            Vector3 lFirstPosition = transform.position;
            Vector3 lKnockBackDestination = transform.position + (pOrigin.forward * KNOCK_BACK_AMOUNT);
            transform.rotation = Quaternion.LookRotation(pOrigin.position- transform.position, Vector3.up);
            while (lTimeStamp > 0)
            {
                transform.position = Vector3.Lerp(lKnockBackDestination, lFirstPosition, lTimeStamp / KNOCK_BACK_TIME);
                lTimeStamp -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }


        public void SetPlayerDevice()
        {
            InputDevice lDevice = DeviceManager.GetInputDevice(playerIndex);
            if (lDevice != null) _playerInput.SwitchCurrentControlScheme(lDevice);
        }
        public void DropTarget()
        {
            _rb.velocity = _rb.velocity / 1.2f;
            _stateMachine.CurrentState = _stateMachine.Wait;
        }
        override protected void Init()
        {
            base.Init();
            isPickableState = false;
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

        protected override void HitElse(Collider pCollider)
        {
            if (isThrown)
            {
                base.HitElse(pCollider);
                _stateMachine.CurrentState = _stateMachine.Running;
            }
        }

        bool isThrown => _stateMachine.CurrentState == _stateMachine.Thrown;

        public ControlScheme getActiveControlScheme() { return _controlScheme; }

        protected override void OnGizmos()
        {
            Gizmos.DrawLine(transform.position + new Vector3(0,0.5f,0), (transform.forward * PICK_UP_DISTANCE+transform.position+new Vector3(0,0.5f,0)));             //GroundCheck debug line
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GROUND_CHECK_DISTANCE, transform.position.z));             //GroundCheck debug line
            base.OnGizmos();
        }

        ///Destroy player's object and delete references
        public void destroyPlayer()
        {
            Destroy(gameObject);
        }
    }
}