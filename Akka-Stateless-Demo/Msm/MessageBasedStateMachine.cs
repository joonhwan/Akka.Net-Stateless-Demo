using System;
using Stateless;

namespace Akka_Stateless_Demo.Msm
{
    public interface ICanHandle
    {
        void Handle(object message);
    }

    public class MessageBasedStateMachine<TState> : ICanHandle
    {
        private StateMachine<TState, Type> _stateMachine;
        private MessageLocator _messageLocator;

        public MessageBasedStateMachine(TState initialState)
        {
            _messageLocator = new MessageLocator();

            _stateMachine = new StateMachine<TState, Type>(initialState);
            _stateMachine.OnTransitioned(transition =>
            {
                OnTransition(transition.Source, transition.Destination, transition.IsReentry, _messageLocator.LastMessage);
            });
            _stateMachine.OnUnhandledTrigger((state, type) =>
            {
                OnUnhandledTrigger(state, _messageLocator.LastMessage);
            });
        }

        public MessageBasedStateMachine(Func<TState> stateAccessor, Action<TState> stateMutator)
        {
            _stateMachine = new StateMachine<TState, Type>(stateAccessor, stateMutator);
        }

        public MessageBasedStateConfiguration<TState> Configure(TState state)
        {
            var internalConfigurator = _stateMachine.Configure(state);
            return new MessageBasedStateConfiguration<TState>(_messageLocator, internalConfigurator);
        }

        public void Handle(object message)
        {
            try
            {
                // message body 를 remember
                _messageLocator.Set(message);

                // meessage type 에 의거하여 sm이 동작.
                _stateMachine.Fire(message.GetType());
            }
            finally
            {
                // meesage body 를 forget
                _messageLocator.Unset();
            }
        }

        public TState State
        {
            get { return _stateMachine.State;  }
        }

        public bool IsInState(TState state)
        {
            return _stateMachine.IsInState(state);
        }

        public string ToDotty()
        {
            return _stateMachine.ToDotGraph();
        }

        protected virtual void OnTransition(TState source, TState destination, bool isReentry, object message)
        {
        }

        protected virtual void OnUnhandledTrigger(TState state, object message)
        {
        }
    }
}
