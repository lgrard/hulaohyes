using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace hulaohyes.states
{
    public abstract class StateMachine
    {
        private State _currentState;

        ///Create a new state machine object
        public StateMachine() { }

        ///Get/Set player's current state
        public State CurrentState
        {
            get => _currentState;
            set
            {
                if(_currentState!=null)_currentState.OnExit();
                _currentState = value;
                _currentState.OnEnter();
            }
        }
    }
}
