using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.levelbrick.checkpoint;

namespace hulaohyes.levelbrick.checkpoint
{
    public class BigCheckpoint : Checkpoint
    {
        [SerializeField] private Transform _secondSpawnPosition;

        public Vector3 SecondSpawnPosition => _secondSpawnPosition.position;
        public Quaternion SecondSpawnRotation => _secondSpawnPosition.rotation;

        protected override void OnGizmos()
        {
            base.OnGizmos();

            if (_secondSpawnPosition != null)
            {
                Gizmos.matrix = Matrix4x4.identity;
                Gizmos.DrawSphere(_secondSpawnPosition.forward * 0.5f + _secondSpawnPosition.position, 0.25f);
                Gizmos.DrawSphere(_secondSpawnPosition.position, 0.5f);
            }
        }
    }
}
