using System;
using System.Collections.Generic;
using Stateless;

namespace Akka_Stateless_Demo.Msm
{
    public class TriggerShooterLocator<TState>
    {   
        private readonly StateMachine<TState, Type> _stateMachine;
        private readonly Dictionary<Type, TriggerShooter<TState>> _typeToTriggerWithParametersMap;

        public TriggerShooterLocator(StateMachine<TState, Type> stateMachine)
        {
            _stateMachine = stateMachine;
            _typeToTriggerWithParametersMap = new Dictionary<Type, TriggerShooter<TState>>();
        }
        
        public ParameterizedTriggerShooter<TState, TTriggerParameter> AddOrGet<TTriggerParameter>()
        {
            var trigger = typeof(TTriggerParameter);

            TriggerShooter<TState> firable;
            if (!_typeToTriggerWithParametersMap.TryGetValue(trigger, out firable))
            {
                firable = new ParameterizedTriggerShooter<TState, TTriggerParameter>()
                          {
                              Trigger = _stateMachine.SetTriggerParameters<TTriggerParameter>(typeof(TTriggerParameter)),
                          };
                _typeToTriggerWithParametersMap[typeof(TTriggerParameter)] = firable;
            }
            return (ParameterizedTriggerShooter<TState, TTriggerParameter>) firable;
        }

        public TriggerShooter<TState>  Get(Type triggerParameterType)
        {
            TriggerShooter<TState> firable;
            if (!_typeToTriggerWithParametersMap.TryGetValue(triggerParameterType, out firable))
            {
                firable = null;
            }
            return firable;
        }
    }
}