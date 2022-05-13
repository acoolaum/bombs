namespace MyCompany.Library.StateMachine
{
    public interface IStateMachine
    {
        IState State { get; }

        void Start(IState state);
    }
}