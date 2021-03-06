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
        const int MAX_HP = 10;

        private NavMeshAgent _navMeshAgent;
        private EnemyStateMachine _stateMachine;
        private Animator _enemyAnimator;
        private SphereCollider _detectionZone;
        private BoxCollider _damageZone;

        [Header("Zone size")]
        [Range(1,10)][SerializeField] float _zoneRadius = 1;
        [SerializeField] Transform _damageZoneSetting;

        [Header("Particles list")]
        [SerializeField] List<ParticleSystem> _enemyParticles;

        [Header("Debug")]
        [SerializeField] string currentState;

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

        private void CreateDamageZone()
        {
            _damageZone = gameObject.AddComponent<BoxCollider>();
            _damageZone.size = _damageZoneSetting.localScale;
            _damageZone.center = _damageZoneSetting.localPosition;
            _damageZone.isTrigger = true;
            _damageZone.enabled = false;
        }

        protected override void Init()
        {
            base.Init();
            isPickableState = false;
            CreateDetectionZone();
            CreateDamageZone();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _enemyAnimator = GetComponent<Animator>();
            _stateMachine = new EnemyStateMachine(this, _rb, _enemyAnimator, _enemyParticles,_navMeshAgent, _detectionZone, _damageZone);
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
            if (isThrown)
            {
                base.HitSomething(pCollider);
                destroyEnemy();
            }

            else if (pCollider.TryGetComponent<PlayerController>(out PlayerController pPlayer))
            {
                if (isAttacking)
                {
                    pPlayer.TakeDamage(1, transform);
                    _stateMachine.CurrentState = _stateMachine.Recovering;
                }

                else if (isIdling && currentTarget == null)
                {
                    currentTarget = pPlayer.transform;
                    _stateMachine.CurrentState = _stateMachine.StartUp;
                }
            }

            else if (_isDropped)
            {
                base.HitSomething(pCollider);
                _navMeshAgent.enabled = true;
                _navMeshAgent.velocity = Vector3.zero;
                _stateMachine.CurrentState = _stateMachine.Idle;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == currentTarget && isRecovering) currentTarget = null;
        }

        public Animator EnemyAnimator => _enemyAnimator;
        private bool isThrown => _stateMachine.CurrentState == _stateMachine.Thrown && !_isDropped;
        private bool isAttacking => _stateMachine.CurrentState == _stateMachine.Attacking;
        private bool isIdling => _stateMachine.CurrentState == _stateMachine.Idle;
        public bool isRecovering => _stateMachine.CurrentState == _stateMachine.Recovering;

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
            Gizmos.color = Color.red;
            if(_damageZoneSetting != null) Gizmos.DrawWireCube(_damageZoneSetting.localPosition, _damageZoneSetting.localScale);
        }
    }
}
