using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.player.states
{
    public class Downed : Wait
    {
        //private const float RESPAWN_TIMER = 5f;

        private PlayerController player;
        private GameManager gameManager;
        private bool disabled;

        public Downed(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles, PlayerController pPlayer)
            : base(pStateMachine, pAnimator, pParticles)
        {
            gameManager = GameManager.getInstance();
            player = pPlayer;
            MAX_TIMER = player.RESPAWN_TIMER;                                      //const to change
            disabled = false;
        }

        override protected void TimerEnd()
        {
            base.TimerEnd();
            gameManager.SpawnPlayer(player.playerIndex);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            disabled = false;
            if (player.pickUpTarget != null) player.pickUpTarget.Drop();
        }

        public override void LoopLogic()
        {
            base.LoopLogic();
            if (Timer <= 4.3f && !player.IsDead && !disabled)
            {
                disabled = true;
                player.Die();
            }
        }
    }
}