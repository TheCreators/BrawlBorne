using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Misc.StateMachine
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        private readonly Dictionary<Type, List<Transition>> _transitions = new();
        private List<Transition> _currentTransitions = new();
        private readonly List<Transition> _anyTransitions = new();
        
        private static readonly List<Transition> EmptyTransitions = new(0);

        public void Tick()
        {
            Transition transition = GetTransition();
            if (transition != null)
            {
                SetState(transition.To);
            }
            
            CurrentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state.Equals(CurrentState))
            {
                return;
            }
            
            CurrentState?.OnExit();
            CurrentState = state;
            
            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;
            
            CurrentState.OnEnter();
        }
        
        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out List<Transition> transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }
        
        public void AddAnyTransition(IState to, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(to, predicate));
        }
        
        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }
            
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }
        
        [CanBeNull]
        private Transition GetTransition()
        {
            foreach (Transition transition in _anyTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }
            
            foreach (Transition transition in _currentTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            return null;
        }
    }
}