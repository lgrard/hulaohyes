using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.player;

namespace hulaohyes.levelbrick
{
    public class Gears : MonoBehaviour
    {
        const float PICKUP_TIME = 0.5f;

        private Animator anim;
        private Collider trigger;

        [Header("Visual")]
        [SerializeField] List<Color> colors;
        [SerializeField] List<Mesh> meshes;
        [SerializeField] List<ParticleSystem> particles;

        [Header("Curves")]
        [SerializeField] AnimationCurve scaleCurve;
        [SerializeField] AnimationCurve positionCurve;

        private void Start()
        {
            trigger = GetComponent<Collider>();

            anim = GetComponent<Animator>();
            anim.SetFloat("Offset", Random.value);

            MeshFilter lMeshFilter = GetComponentInChildren<MeshFilter>();
            int lRandomMeshIndex = Random.Range(0, meshes.Count);
            lMeshFilter.mesh = meshes[lRandomMeshIndex];

            MeshRenderer lMeshRenderer = GetComponentInChildren<MeshRenderer>();
            int lRandomColorIndex = Random.Range(0, colors.Count);
            lMeshRenderer.material.SetColor("GearColor", colors[lRandomColorIndex]);
        }

        IEnumerator PlayerPick(Transform pPlayer)
        {
            float lTimer = PICKUP_TIME;

            while(lTimer > 0)
            {
                float lProgress = lTimer / PICKUP_TIME;
                transform.position = Vector3.Lerp(pPlayer.position, transform.position, positionCurve.Evaluate(lProgress));
                transform.localScale = Vector3.one* scaleCurve.Evaluate(lProgress);

                lTimer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            trigger.enabled = false;
            particles[0].Stop();
            particles[1].Play();
            StartCoroutine(PlayerPick(other.transform));
        }
    }
}
