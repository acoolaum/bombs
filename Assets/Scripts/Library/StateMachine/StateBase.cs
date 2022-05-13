using UnityEngine;

namespace MyCompany.Library.StateMachine
{
    public abstract class StateBase: IState
    {
        protected IStateMachine StateMachine;
        public virtual void Start(IStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            OnStarted();
        }
        public virtual void Stop() { }
        protected abstract void OnStarted();
    }
}