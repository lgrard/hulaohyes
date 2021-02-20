using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using hulaohyes.effects;
using hulaohyes.enemy;

namespace hulaohyes.player.states
{
    public class Attacking : PlayerState
    {
        const float ATTACK_HEIGHT = 1;
        const float ATTACK_DEPTH = 1;
        const float ATTACK_WIDTH = 1;
        const float MAGNET_RADIUS = 3f;

        private float _magnetPositionMagnitude = 2f;
        private LayerMask _enemyLayer;
        private Transform _attackPoint;
        private Vector3 _attackBox = new Vector3(ATTACK_WIDTH, ATTACK_HEIGHT, ATTACK_DEPTH);

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
                Collider magnetTarget = lHitCollidersMagnet[0];
                Debug.Log("Magnet to " + magnetTarget.name);

                Vector3 lLookPosition = magnetTarget.transform.position - _player.transform.position;
                lLookPosition.y = 0;
                _player.transform.rotation = Quaternion.LookRotation(lLookPosition);

                Vector3 lMagnetPosition = magnetTarget.transform.position - (_player.transform.forward * _magnetPositionMagnitude);
                lMagnetPosition.y = _player.transform.position.y;
                _player.transform.position = lMagnetPosition;
            }
        }

        void TriggerPunch()
        {
            _animator.SetTrigger("Punch");
        }

        public void PunchHitTest()
        {
            MagnetCheck();
            Collider[] lHitCollidersDamage = Physics.OverlapBox(_attackPoint.transform.position, _attackBox,_player.transform.rotation, _enemyLayer);
            if (lHitCollidersDamage.Length > 0) _player.StartCoroutine(Effects.HitStop(_animator,_animator,0.15f,0.01f));
            foreach (Collider lHitCollider in lHitCollidersDamage)
            {
                //damage code
                if (lHitCollider.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
                {
                    enemy.StartCoroutine(enemy.KnockBack(_player.transform));
                    enemy.TakeDamage(1);
                }
                Debug.Log(lHitCollider.gameObject.name);
                _particles[1].Play();
            }
        }
        void OnPunch(InputAction.CallbackContext ctx) => TriggerPunch();

        public void ResetCombo()
        {
            _animator.ResetTrigger("Punch");
            base._stateMachine.CurrentState = _stateMachine.running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _rb.velocity = _rb.velocity/2;
            _controlScheme.Player.Punch.performed += OnPunch;
            MagnetCheck();
            TriggerPunch();
        }

        public override void OnExit()
        {
            base.OnExit();
            _controlScheme.Player.Punch.performed -= OnPunch;
        }
    }
}

