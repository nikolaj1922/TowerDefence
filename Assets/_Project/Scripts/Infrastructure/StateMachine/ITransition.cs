using System;

namespace _Project.Scripts.Infrastructure.StateMachine
{
    public interface ITransition
    {
        Type From { get; }
        Type To { get; }
        bool CanTransition();
    }
}