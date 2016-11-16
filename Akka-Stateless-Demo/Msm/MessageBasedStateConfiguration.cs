using System;
using System.Collections.Generic;
using Stateless;

namespace Akka_Stateless_Demo.Msm
{
    public class MessageBasedStateConfiguration<TState>
    {
        private readonly StateMachine<TState, Type>.StateConfiguration _internalConfigurator;
        private readonly TriggerShooterLocator<TState> _triggerShooterLocator;

        public MessageBasedStateConfiguration(
            TriggerShooterLocator<TState> triggerShooterLocator, 
            StateMachine<TState, Type>.StateConfiguration internalConfigurator
            )
        {
            _triggerShooterLocator = triggerShooterLocator;
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
            var shooter = _triggerShooterLocator.AddOrGet<T>();
            _internalConfigurator.PermitDynamic(shooter.Trigger, handler);
            return this;
        }

        public MessageBasedStateConfiguration<TState> OnReceive<T>(Action<T> handler)
        {
            var shooter = _triggerShooterLocator.AddOrGet<T>();
            _internalConfigurator.InternalTransition(shooter.Trigger, (arg, _) => handler(arg));
            return this;
        }

        public void OnReceiveAny(Action<object> handler)
        {
            // no idea yet
            //_internalConfigurator.Machine.OnUnhandledTrigger((state, type) => ??? );
        }
    }
}