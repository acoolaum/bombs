using JetBrains.Annotations;
using MyCompany.Library.Services;

namespace MyCompany.Library.StateMachine
{
    public class StateMachine : ServiceBase, IStateMachine
    {
        public IState State { get; private set; }
        
        public void Start([NotNull] IState state)
        {
            State?.Stop();
            State = state;
            state.Start(this);
        }

        public override void Dispose()
        {
            State?.Stop();
        }
    }
}