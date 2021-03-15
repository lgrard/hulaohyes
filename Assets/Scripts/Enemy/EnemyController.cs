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

        [Header("Values common")]
        public float STARTUP_ROTATION_AMOUNT = 2;                               //const to change
        public float START_UP_DURATION = 3;                                     //const to change
        public float ATTACK_RECOVERING_DURATION = 3f;                                  //const to change
        public float DROP_RECOVER_DURATION = 1f;                

        [Header("Values walker")]
        public float CHARGE_DURATION = 3f;                                      //const to change
        public float CHARGE_ROTATION_AMOUNT = 1f;                               //const to change
        public float CHARGE_SPEED = 6;                                          //const to change

        [Header("Values Turret")]
        public float PROJECTILE_LIFETIME = 3f;                                  //const to change
        public float PROJECTILE_SPEED = 5f;                                     //const to change


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
            enemyAnimator.SetTrigger("Thrown");
            stateMachine.CurrentState = stateMachine.Thrown;
            base.Propel();
        }

        public override void Drop()
        {
            base.isDropped = true;
            enemyAnimator.SetTrigger("Escape");
            stateMachine.CurrentState = stateMachine.Thrown;
            base.Drop();
        }

        override public void GetPicked(PlayerController pPlayer)
        {
            stateMachine.CurrentState = stateMachine.Carried;
            detectionZone.enabled = false;
            base.GetPicked(pPlayer);
        }

        protected override void HitElseThrown(Collider pCollider)
        {
            enemyAnimator.SetTrigger("HitGround");
            destroyEnemy();
        }

        protected override void HitElseDropped(Collider pCollider)
        {
            enemyAnimator.SetTrigger("HitGround");
            stateMachine.CurrentState = stateMachine.Dropped;
        }

        protected virtual void HitPlayer(PlayerController pPlayer)
        {

        }

        protected override void Drowns()
        {
            base.Drowns();
            destroyEnemy();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
            {
                if(currentTarget == null && isIdling)
                {
                    currentTarget = lPlayer.transform;
                    stateMachine.CurrentState = stateMachine.StartUp;
                }

                else if (isAttacking)
                {
                    HitPlayer(lPlayer);
                }
            }
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
            enemyParticles[4].Play();
            enemyAnimator.SetTrigger("TakeDamage");
            _collider.isTrigger = true;
            rb.isKinematic = true;
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
