namespace _Project.Scripts.Infrastructure.StateMachine
{
    public interface IState { }

    public interface IEnterableState : IState
    {
        void Enter();
    }

    public interface IExitableState : IState
    {
        void Exit();   
    }
    
    public interface IUpdatableState : IState
    {
        void Update();   
    }
}