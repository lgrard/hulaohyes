using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using hulaohyes.enemy;

namespace hulaohyes.player.states
{
    public class Attacking : PlayerState
    {
        const float MAX_ATTACK_STAMP = 0.5f;
        const float ATTACK_RADIUS = 1f;
        const float MAGNET_RADIUS = 2f;

        private LayerMask _enemyLayer;
        private float _attackStamp;
        private Transform _attackPoint;

        /// Create a new attacking state object
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pAttackPoint">Hit collision transform</param>
        public Attacking(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator, List<ParticleSystem> pParticles,Transform pAttackPoint)
            : base(pStateMachine, pPlayer, pControlScheme, pCameraContainer, pRb, pAnimator, pParticles)
        {
            _attackPoint = pAttackPoint;
            _enemyLayer = LayerMask.GetMask("Enemy");
        }

        /// Checks for the nearest enemy in range
        void MagnetCheck()
        {
            Collider[] lHitCollidersMagnet = Physics.OverlapSphere(_player.transform.position, MAGNET_RADIUS, _enemyLayer);
            if (lHitCollidersMagnet.Length > 0)
            {
                Debug.Log("Magnet to " + lHitCollidersMagnet[0].name);
                Vector3 lLookPosition = lHitCollidersMagnet[0].transform.position - _player.transform.position;
                lLookPosition.y = 0;
                _player.transform.rotation = Quaternion.LookRotation(lLookPosition);
            }
        }

        void TriggerPunch()
        {
            _animator.SetTrigger("Punch");
        }

        public void PunchHitTest()
        {
            Collider[] lHitCollidersDamage = Physics.OverlapSphere(_attackPoint.transform.position, ATTACK_RADIUS, _enemyLayer);
            foreach(Collider lHitCollider in lHitCollidersDamage)
            {
                //damage code
                if(lHitCollider.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy)) enemy.takeDamage(1);
                Debug.Log(lHitCollider.gameObject.name);
                _particles[1].Play();
            }
        }
        void OnPunch(InputAction.CallbackContext ctx) => TriggerPunch();

        public override void OnEnter()
        {
            base.OnEnter();
            _attackStamp = MAX_ATTACK_STAMP;
            _controlScheme.Player.Punch.performed += OnPunch;
            MagnetCheck();
            TriggerPunch();
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (_attackStamp > 0) _attackStamp -= Time.deltaTime;
            else base._stateMachine.CurrentState = _stateMachine.running;
        }

        public override void OnExit()
        {
            base.OnExit();
            _controlScheme.Player.Punch.performed -= OnPunch;
        }
    }
}

