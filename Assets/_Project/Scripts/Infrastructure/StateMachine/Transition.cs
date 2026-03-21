using System;

namespace _Project.Scripts.Infrastructure.StateMachine
{
    public class Transition<TFrom, TTo> : ITransition 
        where TFrom : IState where TTo : IState
    {
        private readonly Func<bool> _condition;
        public Type From { get; }
        public Type To { get; }
        
        public Transition(Func<bool> condition)
        {
            _condition = condition;
            
            From = typeof(TFrom);
            To = typeof(TTo);
        }

        public bool CanTransition() => _condition();
    }
}