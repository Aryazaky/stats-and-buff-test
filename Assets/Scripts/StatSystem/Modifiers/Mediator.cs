using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    public class Mediator : IEnumerable<Modifier>
    {
        private readonly Action<Modifier> _onModifierAdded;
        private readonly Action<Modifier> _onModifierRemoved;
        private readonly List<Modifier> _modifiers = new();

        public Mediator(Action<Modifier> onModifierAdded = null, Action<Modifier> onModifierRemoved = null)
        {
            _onModifierAdded = onModifierAdded;
            _onModifierRemoved = onModifierRemoved;
        }

        internal void PerformQuery(IQuery query)
        {
            foreach (var modifier in _modifiers.ToArray())
            {
                modifier.Handle(query);
            }

            RemoveModifiers(mod => mod.IsExpired);
        }

        public void AddModifier(Modifier modifier)
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

        public void RemoveModifiers(Func<Modifier, bool> predicate)
        {
            foreach (var modifier in _modifiers.Where(predicate).ToArray())
            {
                modifier.Dispose();
            }
        }

        public IEnumerator<Modifier> GetEnumerator() => _modifiers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_modifiers).GetEnumerator();
    }
}