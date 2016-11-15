using System;
using Stateless;

namespace Akka_Stateless_Demo.Msm
{
    public class MessageBasedStateConfiguration<TState>
    {
        private readonly StateMachine<TState, Type>.StateConfiguration _internalConfigurator;
        private readonly MessageLocator _messageLocator;

        public MessageBasedStateConfiguration(MessageLocator messageLocator, StateMachine<TState, Type>.StateConfiguration internalConfigurator)
        {
            _messageLocator = messageLocator;
            _internalConfigurator = internalConfigurator;
        }

        public MessageBasedStateConfiguration<TState> OnEntry(Action entryAction, string entryActionDescription = null)
        {
            _internalConfigurator.OnEntry(entryAction, entryActionDescription);
            return this;
        }

        public MessageBasedStateConfiguration<TState> OnExit(Action exitAction, string exitActionDescription = null)
        {
            _internalConfigurator.OnExit(exitAction, exitActionDescription);
            return this;
        }

        public MessageBasedStateConfiguration<TState> SubstateOf(TState superState)
        {
            _internalConfigurator.SubstateOf(superState);
            return this;
        }

        public MessageBasedStateConfiguration<TState> OnReceive<T>(Func<T, TState> handler)
        {
            _internalConfigurator.PermitDynamicIf(typeof(T), () => HandleMessage(handler), () => CanHandle(handler), null);
            return this;
        }

        public MessageBasedStateConfiguration<TState> OnReceive<T>(Action<T> handler)
        {
            _internalConfigurator.IgnoreIf(typeof(T), () =>
            {
                HandleMessage(handler);
                return true; // no - transition.
            });
            return this;
        }

        public void OnReceiveAny(Action<object> handler)
        {
            // no idea yet
            throw new NotImplementedException();
        }

        private bool CanHandle<T>(Func<T, TState> handler)
        {
            T message;
            return _messageLocator.TryGet(out message);
        }

        private TState HandleMessage<T>(Func<T, TState> handler)
        {
            T message;
            if (!_messageLocator.TryGet(out message))
            {
                throw new InvalidOperationException("Message Type Mismatched!");
            }
            return handler(message);
        }

        private void HandleMessage<T>(Action<T> handler)
        {
            T message;
            if (!_messageLocator.TryGet(out message))
            {
                throw new InvalidOperationException("Message Type Mismatched!");
            }
            handler(message);
        }

    }
}