using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.enemy.states.walker;

namespace hulaohyes.enemy
{
    public class WalkerController : EnemyController
    {
        private BoxCollider _damageZone;
        [SerializeField] Transform _damageZoneSetting;

        protected override void Init()
        {
            base.Init();
            CreateDamageZone();
            _stateMachine = new WalkerStateMachine(this, _rb, _enemyAnimator, _enemyParticles, _navMeshAgent, _detectionZone, _damageZone);
        }

        private void CreateDamageZone()
        {
            _damageZone = gameObject.AddComponent<BoxCollider>();
            _damageZone.size = _damageZoneSetting.localScale;
            _damageZone.center = _damageZoneSetting.localPosition;
            _damageZone.isTrigger = true;
            _damageZone.enabled = false;
        }

        protected override void OnGizmos()
        {
            base.OnGizmos();
            Gizmos.color = Color.red;
            if (_damageZoneSetting != null) Gizmos.DrawWireCube(_damageZoneSetting.localPosition, _damageZoneSetting.localScale);
        }
    }
}
