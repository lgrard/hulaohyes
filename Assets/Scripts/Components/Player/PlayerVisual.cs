using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.Assets.Scripts.Targettable;

namespace hulaohyes.Assets.Scripts.Components.Player
{
    public class PlayerVisual : PlayerComponent
    {
        [Header("Particles")]
        [SerializeField] private ParticleSystem jumpParticles;
        [SerializeField] private ParticleSystem changeDirectionParticles;
        [SerializeField] private Animator animator;

        private int lastDirection = 0;

        void Start()
        {
            player.onJump += OnJump;
            player.onTargetAquire += OnTargetAquire;
            player.onTargetLoss += OnTargetLoss;
            player.onChangeDirection += OnChangeDirection;
            player.onPickUp += OnPickUp;
            player.onThrow += OnThrow;
            player.onDrop += OnDrop;
            player.onInteract += OnInteract;

            OnChangeDirection(1);
        }

        private void Update()
        {
            SpeedSet();
            GroundCheck();
        }

        void SpeedSet()
        {
            float lSpeedRatio = player.rb.velocity.magnitude / player.playerDataSet.movementSpeed;
            animator?.SetFloat("Speed", lSpeedRatio);
        }

        void GroundCheck()
        {
            animator?.SetBool("isGrounded",player.isGrounded);
        }

        void OnChangeDirection(int pDirection)
        {
            if(pDirection != lastDirection)
            {
                lastDirection = pDirection;

                float lDirection = lastDirection > 0 ? 90 : -90;
                animator.gameObject.transform.eulerAngles = new Vector3(0, lDirection, animator.gameObject.transform.eulerAngles.z);

                if(!changeDirectionParticles.isPlaying) changeDirectionParticles?.Play();
            }
        }

        void OnJump()
        {
            animator?.SetTrigger("Jump");
            jumpParticles?.Play();
        }

        void OnTargetAquire()
        {
            animator.SetBool("CanPick", true);
        }

        void OnTargetLoss()
        {
            animator.SetBool("CanPick", false);
        }

        void OnPickUp()
        {
            animator.SetTrigger("PressPick");
            animator.SetBool("CanPick", false);
            animator.SetBool("Carrying", true);
        }

        void OnThrow()
        {
            animator.SetTrigger("Throw");
            animator.SetBool("Carrying", false);
        }

        void OnDrop()
        {
            animator.SetBool("Carrying", false);
        }

        void OnInteract()
        {
            animator.SetTrigger("PressPick");
            animator.SetBool("CanPick", false);
        }
    }
}
