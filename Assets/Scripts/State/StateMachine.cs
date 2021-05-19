namespace hulaohyes.Assets.Scripts.State
{
    public class StateMachine
    {
        protected IState _currentState;

        public IState SetState(IState pNewState)
        {
            currentState?.OnExit();
            _currentState = pNewState;
            currentState?.OnEnter();
            return pNewState;
        }

        public IState currentState => _currentState;
    }
}