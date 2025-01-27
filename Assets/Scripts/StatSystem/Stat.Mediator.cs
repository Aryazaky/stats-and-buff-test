using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public readonly partial struct Stat
    {
        public class Mediator
        {
            private readonly List<Modifier> _modifiers = new();

            public void PerformQuery(QueryArgs queryArgs)
            {
                foreach (var modifier in _modifiers.ToArray())
                {
                    modifier.Handle(queryArgs);
                }

                RemoveModifiers(mod => mod.IsExpired);
            }

            public void AddModifier(Modifier modifier)
            {
                _modifiers.Add(modifier);
                _modifiers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                modifier.OnDispose += mod => _modifiers.Remove(mod);
            }

            public void RemoveModifiers(Func<Modifier, bool> predicate)
            {
                foreach (var modifier in _modifiers.Where(predicate).ToArray())
                {
                    modifier.Dispose();
                }
            }
        }
    }
}