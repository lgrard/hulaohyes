using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.checkpoint
{
    public class PlayerStart : MonoBehaviour
    {
        static PlayerStart _instance;

        [Header("Player spawn")]
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private Transform _secondSpawnPosition;

        [SerializeField] bool drawGizmos =true;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else
            {
                Debug.Log("Attempt to create a second PlayerStart");
                Destroy(this.gameObject);
            }
        }

        public static PlayerStart getInstance()
        {
            if (_instance != null) return _instance;
            else
            {
                Debug.LogError("No PlayerStart on scene, you need to had one");
                return null;
            }
        }

        public Vector3 SpawnPosition => _spawnPosition.position;
        public Quaternion SpawnRotation => _spawnPosition.rotation;
        public Vector3 SecondSpawnPosition => _secondSpawnPosition.position;
        public Quaternion SecondSpawnRotation => _secondSpawnPosition.rotation;

        private void OnDestroy() => _instance = null;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (drawGizmos)
            {
                if (_spawnPosition != null)
                {
                    Gizmos.DrawSphere(_spawnPosition.forward * 0.5f + _spawnPosition.position, 0.25f);
                    Gizmos.DrawSphere(_spawnPosition.position, 0.5f);
                }

                if (_secondSpawnPosition != null)
                {
                    Gizmos.DrawSphere(_secondSpawnPosition.forward * 0.5f + _secondSpawnPosition.position, 0.25f);
                    Gizmos.DrawSphere(_secondSpawnPosition.position, 0.5f);
                }
            }
        }
    }
}