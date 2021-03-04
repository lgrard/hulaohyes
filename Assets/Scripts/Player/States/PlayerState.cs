using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public abstract class PlayerState:State
    {
        protected PlayerStateMachine _stateMachine;
        protected Animator _animator;
        protected List<ParticleSystem> _particles;

        /// Create a new state
        /// <param name="pAnimator">Associated animator component</param>
        /// <param name="pParticles">Associated particles list</param>
        public PlayerState(PlayerStateMachine pStateMachine, Animator pAnimator, List<ParticleSystem> pParticles):base()
        {
            _stateMachine = pStateMachine;
            _animator = pAnimator;
            _particles = pParticles;
        }
    }
}