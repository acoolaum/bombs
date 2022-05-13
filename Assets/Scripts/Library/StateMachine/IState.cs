namespace MyCompany.Library.StateMachine
{
    public interface IState
    {
        void Start(IStateMachine stateMachine);
        void Stop();
    }
}