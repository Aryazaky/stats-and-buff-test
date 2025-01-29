using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public class Mediator
    {
        private readonly Action<Stat.Modifier> _onModifierAdded;
        private readonly Action<Stat.Modifier> _onModifierRemoved;
        private readonly List<Stat.Modifier> _modifiers = new();

        public Mediator(Action<Stat.Modifier> onModifierAdded = null, Action<Stat.Modifier> onModifierRemoved = null)
        {
            _onModifierAdded = onModifierAdded;
            _onModifierRemoved = onModifierRemoved;
        }

        public void PerformQuery(StatQuery query)
        {
            foreach (var modifier in _modifiers.ToArray())
            {
                modifier.Handle(query);
            }

            RemoveModifiers(mod => mod.IsExpired);
        }

        public void AddModifier(Stat.Modifier modifier)
        {
            if (modifier == null)
            {
                throw new Exception("Modifier cannot be null");
            }
            _modifiers.Add(modifier);
            _modifiers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            modifier.OnDispose += mod =>
            {
                _modifiers.Remove(mod);
                _onModifierRemoved?.Invoke(modifier);
            };
            _onModifierAdded?.Invoke(modifier);
        }

        public void RemoveModifiers(Func<Stat.Modifier, bool> predicate)
        {
            foreach (var modifier in _modifiers.Where(predicate).ToArray())
            {
                modifier.Dispose();
            }
        }
    }
}