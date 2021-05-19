using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;

namespace hulaohyes.states
{
    public abstract class State
    {
        /// Create a new state
        public State() { }

        public virtual void OnEnter() { }
        public virtual void LoopLogic() { } 
        public virtual void PhysLoopLogic() { }
        public virtual void OnExit() { }
    }
}