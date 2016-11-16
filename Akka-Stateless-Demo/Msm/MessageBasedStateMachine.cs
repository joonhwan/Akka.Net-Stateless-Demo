using System;
using System.Collections.Generic;
using System.Reflection;
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
        private TriggerShooterLocator<TState> _triggerShooterLocator;

        public MessageBasedStateMachine(TState initialState)
        {
            _stateMachine = new StateMachine<TState, Type>(initialState);
            _triggerShooterLocator = new TriggerShooterLocator<TState>(_stateMachine);

            _stateMachine.OnTransitioned(transition =>
            {
                Transitioned?.Invoke(new TransitionInfo<TState>(transition.Source, transition.Destination, transition.IsReentry, LastMessage));
            });
            _stateMachine.OnUnhandledTrigger((state, type) =>
            {
                Unhandled?.Invoke(state, LastMessage);
            });
        }

        public object LastMessage { get; set; }

        public Action<TransitionInfo<TState>> Transitioned;
        public Action<TState, object> Unhandled;

        public MessageBasedStateMachine(Func<TState> stateAccessor, Action<TState> stateMutator)
        {
            _stateMachine = new StateMachine<TState, Type>(stateAccessor, stateMutator);
        }

        public MessageBasedStateConfiguration<TState> Configure(TState state)
        {
            var internalConfigurator = _stateMachine.Configure(state);
            return new MessageBasedStateConfiguration<TState>(_triggerShooterLocator, internalConfigurator);
        }

        public void Handle(object message)
        {
            var shooter = _triggerShooterLocator.Get(message.GetType());
            if (shooter != null)
            {
                try
                {
                    LastMessage = message;
                    shooter.Fire(_stateMachine, message);
                }
                finally
                {
                    LastMessage = null;
                }
            }
            else
            {
                Unhandled?.Invoke(_stateMachine.State, message);
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

        protected virtual void OnUnhandledTrigger(TState state, object message)
        {
        }

    }

    public class TransitionInfo<TState>
    {
        public TransitionInfo(TState source, TState destination, bool isReentry, object lastMessage)
        {
            Source = source;
            Destination = destination;
            IsReentry = isReentry;
            LastMessage = lastMessage;
        }

        public TState Source { get; private set; }
        public TState Destination { get; private set; }
        public bool IsReentry { get; private set; }
        public object LastMessage { get; private set; }

        public override string ToString()
        {
            return $"Source: {Source}, Destination: {Destination}, IsReentry: {IsReentry}, LastMessage: {LastMessage}";
        }
    }
}
