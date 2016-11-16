using System;
using Stateless;

namespace Akka_Stateless_Demo.Msm
{
    public abstract class TriggerShooter<TState>
    {
        public void Fire(StateMachine<TState, Type> sm, object triggerParameter)
        {
            TypedFire(sm, triggerParameter);
        }

        protected abstract void TypedFire(StateMachine<TState, Type> sm, object triggerParameter);
    }

    public class ParameterizedTriggerShooter<TState, TTriggerParameter> : TriggerShooter<TState>
    {
        protected override void TypedFire(StateMachine<TState, Type> sm, object triggerParameter)
        {
            sm.Fire(Trigger, (TTriggerParameter)triggerParameter);
        }

        public StateMachine<TState, Type>.TriggerWithParameters<TTriggerParameter> Trigger { get; set; }
    }   
}