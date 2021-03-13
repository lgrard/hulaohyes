using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Downed : Wait
    {
        private PlayerController player;
        private GameManager gameManager;
        private bool playerDead = false;

        public Downed(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles, PlayerController pPlayer)
            : base(pStateMachine, pAnimator, pParticles)
        {
            gameManager = GameManager.getInstance();
            player = pPlayer;
            MAX_TIMER = 5;
        }

        override protected void TimerEnd()
        {
            base.TimerEnd();
            gameManager.SpawnPlayer(player.playerIndex);
            stateMachine.CurrentState = stateMachine.Running;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            playerDead = false;
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (Timer <= 4.3f && !playerDead)
            {
                playerDead = true;
                player.Die();
            }
        }
    }
}