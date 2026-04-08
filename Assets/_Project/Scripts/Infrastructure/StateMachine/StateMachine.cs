using System;
using System.Linq;

namespace _Project.Scripts.Infrastructure.StateMachine
{
    public class StateMachine
    {
        private IState _currentState;
        private readonly IState[] _states;
        private readonly ITransition[] _transitions;

        public StateMachine(IState[] states, ITransition[] transitions)
        {
            _states = states;
            _transitions = transitions;
            _currentState = _states[0];

            if (_currentState is IEnterableState enterable)
                enterable.Enter();
        }

        public void Update()
        {
            if(_currentState is IUpdatableState updatable)
                updatable.Update();
            
            foreach (ITransition transition in _transitions)
            {
                if (transition.From == _currentState.GetType() && transition.CanTransition())
                    SwitchState(transition.To);
            }
        }

        public void ResetState()
        {
            _currentState = _states[0];

            if (_currentState is IEnterableState enterable)
                enterable.Enter();
        }

        public void SetState(Type to) => SwitchState(to);
        
        private void SwitchState(Type to)
        {
            if(_currentState is IExitableState exitableState)
                exitableState.Exit();
            
            _currentState = _states.First(s => s.GetType() == to);

            if (_currentState is IEnterableState enterableState)
                enterableState.Enter();
        }
    }
}