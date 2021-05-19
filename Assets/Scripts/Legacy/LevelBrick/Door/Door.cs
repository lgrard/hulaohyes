using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.levelbrick.door
{
    public class Door : MonoBehaviour
    {
        private DoorManager _doorManager;
        private MeshRenderer _doorRenderer;
        private Color _color;
        [SerializeField] private GameObject _moveDoor;
        [SerializeField] private ParticleSystem openParticles;

        void Start()
        {
            _doorRenderer = _moveDoor.GetComponent<MeshRenderer>();
            if (_doorRenderer != null)
            {
                _doorRenderer.materials[0].SetColor("EmissiveColor", _color);
                _doorRenderer.materials[1].SetColor("EmitColor", _color);

                if (_doorManager.UnitRequirement>1)
                    _doorRenderer.materials[2].SetColor("EmitColor", _color);
            }
        }

        public ParticleSystem OpenParticles => openParticles;
        public MeshRenderer DoorRenderer => _doorRenderer;
        public GameObject moveDoor => _moveDoor;
        public DoorManager DoorManager
        {
            set => _doorManager = value;
        }
        public Color Color
        {
            set => _color = value;
        }
    }
}
