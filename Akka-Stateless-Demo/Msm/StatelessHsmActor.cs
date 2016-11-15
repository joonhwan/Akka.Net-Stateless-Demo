using Akka.Actor;

namespace Akka_Stateless_Demo.Msm
{
    public abstract class StatelessHsmActor<TState> : ReceiveActor
    {
        private MessageBasedStateMachine<TState> _sm;
        private readonly TState _initState;
        
        public StatelessHsmActor(TState initState)
        {
            _initState = initState;
            
            ReceiveAny(o => _sm.Handle(o));
        }

        protected override void PreStart()
        {
            base.PreStart();

            var initState = _initState; // or from persistence.?!
            _sm = new MessageBasedStateMachine<TState>(_initState);
            ConfigureStateMachine(_sm);
        }

        protected abstract void ConfigureStateMachine(MessageBasedStateMachine<TState> sm);
    }
}