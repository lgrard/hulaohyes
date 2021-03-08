using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.enemy.states;
using hulaohyes.player;

namespace hulaohyes.enemy
{
    public class EnemyController : Pickable
    {
        protected NavMeshAgent _navMeshAgent;
        protected EnemyStateMachine _stateMachine;
        protected Animator _enemyAnimator;
        protected SphereCollider _detectionZone;

        [Header("Zone size")]
        [Range(1,10)][SerializeField] float _zoneRadius = 1;

        [Header("Particles list")]
        [SerializeField] protected List<ParticleSystem> _enemyParticles;

        [Header("Debug")]
        [SerializeField] string currentState;

        [HideInInspector]
        public Transform currentTarget;

        private void Start() => Init();

        private void Update() => _stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            currentState = _stateMachine.CurrentState.ToString();
            _stateMachine.CurrentState.PhysLoopLogic();
            _rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);
        }

        private void CreateDetectionZone()
        {
            _detectionZone = gameObject.AddComponent<SphereCollider>();
            _detectionZone.radius = _zoneRadius;
            _detectionZone.isTrigger = true;
            _detectionZone.enabled = false;
        }

        protected virtual void HitPlayer(PlayerController pPlayer)
        {
            if (isIdling && currentTarget == null)
            {
                currentTarget = pPlayer.transform;
                _stateMachine.CurrentState = _stateMachine.StartUp;
            }
        }

        protected override void Init()
        {
            base.Init();
            isPickableState = false;
            CreateDetectionZone();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _enemyAnimator = GetComponent<Animator>();
            GameManager.AddEnemy(this);
        }

        public override void Propel()
        {
            _isDropped = false;
            _stateMachine.CurrentState = _stateMachine.Thrown;
            base.Propel();
        }

        public override void Drop()
        {
            _isDropped = true;
            _stateMachine.CurrentState = _stateMachine.Thrown;
            base.Drop();
        }

        override public void GetPicked(PlayerController pPlayer)
        {
            _stateMachine.CurrentState = _stateMachine.Carried;
            _detectionZone.enabled = false;
            _navMeshAgent.enabled = false;
            base.GetPicked(pPlayer);
        }

        protected override void HitSomething(Collider pCollider)
        {
            base.HitSomething(pCollider);
            if (pCollider.TryGetComponent<PlayerController>(out PlayerController pPlayer))
                HitPlayer(pPlayer);
        }

        protected override void HitElseThrown(Collider pCollider)
        {
            destroyEnemy();
        }

        protected override void HitElseDropped(Collider pCollider)
        {
            _navMeshAgent.enabled = true;
            _stateMachine.CurrentState = _stateMachine.Idle;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == currentTarget && isRecovering) currentTarget = null;
        }

        public Animator EnemyAnimator => _enemyAnimator;
        private bool isThrown => _stateMachine.CurrentState == _stateMachine.Thrown && !_isDropped;
        private bool isDropped => _stateMachine.CurrentState == _stateMachine.Thrown && _isDropped;
        private bool isIdling => _stateMachine.CurrentState == _stateMachine.Idle;
        public bool isRecovering => _stateMachine.CurrentState == _stateMachine.Recovering;
        protected bool isAttacking => _stateMachine.CurrentState == _stateMachine.Attacking;


        public void destroyEnemy()
        {
            _enemyAnimator.SetTrigger("TakeDamage");
            _rb.isKinematic = true;
            Debug.Log(gameObject.name + " is dead");
            Destroy(gameObject, 0.5f);
        }

        protected override void OnGizmos()
        {
            base.OnGizmos();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Vector3.zero, _zoneRadius);
        }
    }
}
