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
        protected EnemyStateMachine stateMachine;
        protected Animator enemyAnimator;
        protected SphereCollider detectionZone;

        [Header("Zone size")]
        [Range(1,10)][SerializeField] float zoneRadius = 1;

        [Header("Particles list")]
        [SerializeField] protected List<ParticleSystem> enemyParticles;

        [Header("Debug")]
        [SerializeField] string currentState;

        [HideInInspector]
        public Transform currentTarget;

        private void Start() => Init();

        private void Update() => stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            currentState = stateMachine.CurrentState.ToString();
            stateMachine.CurrentState.PhysLoopLogic();
            rb.AddForce(Physics.gravity * _gravity, ForceMode.Acceleration);
        }

        private void CreateDetectionZone()
        {
            detectionZone = gameObject.AddComponent<SphereCollider>();
            detectionZone.radius = zoneRadius;
            detectionZone.isTrigger = true;
            detectionZone.enabled = false;
        }

        protected virtual void HitPlayer(PlayerController pPlayer)
        {
            if (isIdling && currentTarget == null)
            {
                currentTarget = pPlayer.transform;
                stateMachine.CurrentState = stateMachine.StartUp;
            }
        }

        protected override void Init()
        {
            base.Init();
            CreateDetectionZone();
            enemyAnimator = GetComponent<Animator>();
            GameManager.AddEnemy(this);
        }

        public override void Propel()
        {
            base.isDropped = false;
            stateMachine.CurrentState = stateMachine.Thrown;
            base.Propel();
        }

        public override void Drop()
        {
            base.isDropped = true;
            stateMachine.CurrentState = stateMachine.Thrown;
            base.Drop();
        }

        override public void GetPicked(PlayerController pPlayer)
        {
            stateMachine.CurrentState = stateMachine.Carried;
            detectionZone.enabled = false;
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
            enemyAnimator.SetTrigger("Escape");
            stateMachine.CurrentState = stateMachine.Idle;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == currentTarget && isRecovering) currentTarget = null;
        }

        public Animator EnemyAnimator => enemyAnimator;
        private bool isThrown => stateMachine.CurrentState == stateMachine.Thrown && !base.isDropped;
        private bool isDroppedEnemy => stateMachine.CurrentState == stateMachine.Thrown && base.isDropped;
        private bool isIdling => stateMachine.CurrentState == stateMachine.Idle;
        public bool isRecovering => stateMachine.CurrentState == stateMachine.Recovering;
        protected bool isAttacking => stateMachine.CurrentState == stateMachine.Attacking;


        public void destroyEnemy()
        {
            enemyAnimator.SetTrigger("TakeDamage");
            rb.isKinematic = true;
            Debug.Log(gameObject.name + " is dead");
            Destroy(gameObject, 0.5f);
        }

        protected override void OnGizmos()
        {
            base.OnGizmos();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Vector3.zero, zoneRadius);
        }
    }
}
