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
        const float KNOCK_BACK_AMOUNT = 0.3f;
        const float KNOCK_BACK_TIME = 0.1f;

        private NavMeshAgent _navMeshAgent;
        private EnemyStateMachine _stateMachine;
        private Animator _enemyAnimator;
        private SphereCollider _detectionZone;

        [Header("Zone size")]
        [Range(1,10)][SerializeField] float _zoneRadius = 1;

        [Header("Particles list")]
        [SerializeField] List<ParticleSystem> _enemyParticles;
        
        public Transform currentTarget;

        private void Start() => Init();

        public IEnumerator KnockBack(Transform pOrigin)
        {
            float lTimeStamp = KNOCK_BACK_TIME;
            Vector3 lFirstPosition = transform.position;
            Vector3 lKnockBackDestination = transform.position + (pOrigin.forward * KNOCK_BACK_AMOUNT);
            while (lTimeStamp > 0)
            {
                transform.position = Vector3.Lerp(lKnockBackDestination, lFirstPosition, lTimeStamp/ KNOCK_BACK_TIME);
                lTimeStamp -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void Update() => _stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            _stateMachine.CurrentState.PhysLoopLogic();
            _rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);
        }

        public Animator EnemyAnimator => _enemyAnimator;

        private void CreateZone()
        {
            _detectionZone = gameObject.AddComponent<SphereCollider>();
            _detectionZone.radius = _zoneRadius;
            _detectionZone.isTrigger = true;
        }

        protected override void Init()
        {
            base.Init();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _enemyAnimator = GetComponent<Animator>();
            _stateMachine = new EnemyStateMachine(this, _rb, _enemyAnimator, _enemyParticles,_navMeshAgent);
            CreateZone();
            GameManager.AddEnemy(this);
        }

        public override void Propel()
        {
            _stateMachine.CurrentState = _stateMachine.Thrown;
            base.Propel();
        }

        override public void GetPicked(PlayerController pPlayer)
        {
            _stateMachine.CurrentState = _stateMachine.Carried;
            _detectionZone.enabled = false;
            _navMeshAgent.enabled = false;
            base.GetPicked(pPlayer);
        }

        protected override void HitSomething()
        {
            if(_stateMachine.CurrentState == _stateMachine.Thrown)
            {
                base.HitSomething();
                destroyEnemy();
            }
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if(currentTarget == null && other.TryGetComponent<PlayerController>(out PlayerController pPlayer)
                && _stateMachine.CurrentState != _stateMachine.Carried)
            {
                currentTarget = pPlayer.transform;
                _stateMachine.CurrentState = _stateMachine.Attacking;
            }
        }*/

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == currentTarget)
            {
                _stateMachine.CurrentState = _stateMachine.Idle;
                currentTarget = null;
            }
        }

        public void destroyEnemy()
        {
            Debug.Log(gameObject.name + " is dead");
            _rb.isKinematic = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
