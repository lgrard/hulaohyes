using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;

namespace hulaohyes.levelbrick.checkpoint
{
    public class Checkpoint : MonoBehaviour
    {
        private GameManager gameManager;
        private BoxCollider triggerZone;

        [Header("This checkpoint index")]
        [SerializeField] private int _cpIndex;

        [Header("Values")]
        [SerializeField] private Vector3 triggerZoneSize;
        [SerializeField] private bool drawGizmos = true;

        [Header("Player spawn")]
        [SerializeField] private Transform _spawnPosition;

        private void Start() => Init();

        protected virtual void Init()
        {
            gameManager = GameManager.getInstance();
            if (this is Checkpoint || this is BigCheckpoint) triggerZone = CreateTrigger();
        }

        public BoxCollider CreateTrigger()
        {
            BoxCollider lTrigger = gameObject.AddComponent<BoxCollider>();
            lTrigger.isTrigger = true;
            lTrigger.center = new Vector3(0, triggerZoneSize.y / 2, 0);
            lTrigger.size = triggerZoneSize;
            return lTrigger;
        }

        public Vector3 SpawnPosition => _spawnPosition.position;
        public Quaternion SpawnRotation => _spawnPosition.rotation;
        public int CheckPointIndex => _cpIndex;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<PlayerController>(out PlayerController lPlayer))
                gameManager.SetCurrentSpawner(lPlayer.playerIndex,this);
        }

        private void OnDisable()
        {
            triggerZone.enabled = false;
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos) OnGizmos();
        }

        protected virtual void OnGizmos()
        {
            Gizmos.color = this.enabled ? Color.green : Color.red;

            if (this is Checkpoint || this is BigCheckpoint)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawWireCube(new Vector3(0, triggerZoneSize.y / 2, 0), triggerZoneSize);
            }

            if (_spawnPosition != null)
            {
                Gizmos.matrix = Matrix4x4.identity;
                Gizmos.DrawSphere(_spawnPosition.forward*0.5f+_spawnPosition.position, 0.25f);
                Gizmos.DrawSphere(_spawnPosition.position, 0.5f);
            }
        }
    }
}
