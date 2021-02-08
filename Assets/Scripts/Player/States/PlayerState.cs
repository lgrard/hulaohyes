using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hulaohyes.states;

namespace hulaohyes.player.states
{
    public abstract class PlayerState:State
    {
        protected PlayerStateMachine _stateMachine;
        protected PlayerController _player;
        protected ControlScheme _controlScheme;
        protected Transform _cameraContainer;
        protected Rigidbody _rb;
        protected Animator _animator;

        /// Create a new state
        /// <param name="pStateMachine">Associated state machine</param>
        /// <param name="pPlayer">Associated player controller</param>
        /// <param name="pControlScheme">Associated input component</param>
        /// <param name="pCameraContainer">Associated camera container</param>
        /// <param name="pRb">Associated rigidbody</param>
        /// <param name="pAnimator">Associated animator component</param>
        public PlayerState(PlayerStateMachine pStateMachine, PlayerController pPlayer, ControlScheme pControlScheme,
            Transform pCameraContainer, Rigidbody pRb, Animator pAnimator)
            :base()
        {
            _stateMachine = pStateMachine;
            _player = pPlayer;
            _cameraContainer = pCameraContainer;
            _controlScheme = pControlScheme;
            _rb = pRb;
            _animator = pAnimator;
        }
    }
}