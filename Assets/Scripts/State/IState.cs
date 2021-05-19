namespace hulaohyes.Assets.Scripts.State
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void PhysTick();
        void Tick();
        void LateTick();
    }
}