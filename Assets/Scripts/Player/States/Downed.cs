using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Downed : Wait
    {
        private PlayerController player;
        private GameManager gameManager;

        public Downed(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles, PlayerController pPlayer)
            : base(pStateMachine, pAnimator, pParticles)
        {
            gameManager = GameManager.getInstance();
            player = pPlayer;
            MAX_TIMER = 3;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.SetBool("Downed", true);
        }

        override protected void TimerEnd()
        {
            base.TimerEnd();
            gameManager.SpawnPlayer(player.playerIndex);
        }

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("Downed", false);
        }
    }
}