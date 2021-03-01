using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using hulaohyes.enemy.states;

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
        private Transform _currentTarget;

        private int _hp = 1;

        [Header("Particles list")]
        [SerializeField] List<ParticleSystem> _enemyParticles;

        [Header("Current pick up target")]
        public Pickable pickUpTarget;

        [Header("Angles")]
        [Range(0,359)][SerializeField] float _grabAngle = 45;
        [Range(0, 10)][SerializeField] float _grabRadius = 1;
        [Range(0, 359)][SerializeField] float _viewAngle = 45;
        [Range(0, 50)][SerializeField] float _viewRadius =1;
        [Range(0, 50)] [SerializeField] float _meleeRadius = 1;


        private void Start() => Init();

        /// The enemy takes damage and check the amount of HPs
        /// <param name="damage">Amount of damage taken by the enemy</param>
        public void TakeDamage(int pDamage)
        {
            if (_hp - pDamage <= 0)
            {
                //dies
                Debug.Log(gameObject.name+" is dead");
                _enemyAnimator.SetTrigger("TakeDamage");
            }

            else
            {
                _hp -= pDamage;
                _enemyAnimator.SetTrigger("TakeDamage");
            }
        }

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

        //private void Update() => _stateMachine.CurrentState.LoopLogic();
        private void FixedUpdate()
        {
            //_stateMachine.CurrentState.PhysLoopLogic();
            _rb.AddForce(Physics.gravity * 4, ForceMode.Acceleration);
        }

        protected override void Init()
        {
            base.Init();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _enemyAnimator = GetComponent<Animator>();
            _stateMachine = new EnemyStateMachine(this, _rb, _enemyAnimator, null, _enemyParticles,_navMeshAgent);
            GameManager.AddEnemy(this);

            _hp = MAX_HP;
        }

        public override void GetPicked()
        {
            base.GetPicked();
            _navMeshAgent.enabled = false;
        }

        public float GrabAngles { get => _grabAngle; }
        public float GrabRadius { get => _grabRadius; }
        public float ViewAngles { get => _viewAngle; }
        public float ViewRadius { get => _viewRadius; }
        public float MeleeRadius { get => _meleeRadius; }


        public void destroyEnemy()
        {
            //Destructor
        }
    }
}
