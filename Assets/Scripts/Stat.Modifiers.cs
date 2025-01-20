using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public readonly partial struct Stat
{
    public abstract class Modifier : IDisposable
    {
        public static class PriorityType
        {
            public const int Default = 0;
            public const int Boost = 1; // Equipment stat boosts and such. They're additive
            public const int Multiplicative = 2; // Buffs or debuffs
            public const int Offset = 3; // This is also additive, but done normally. For flat stat increase/decrease unaffected by buffs
            public const int Override = 4; // Evil, the ultimate No U. Could set the hard work we all did to calculate all that to 0 for all I know. 
        }

        public readonly struct Contexts
        {
            public readonly object Sender;
            public readonly Query Query;
            public readonly Modifier Modifier;

            public Contexts(object sender, Query query, Modifier modifier)
            {
                Sender = sender;
                Query = query;
                Modifier = modifier;
            }
        }

        public delegate void Operation(Contexts contexts);
        public delegate bool ActivePrerequisite(Contexts contexts); // To allow enable or disable without expiring the modifier
        public event Action<Modifier> OnDispose;
        public event Action<Modifier> OnInvoke;
        public event Action<Modifier> OnInvokeFail;

        private readonly Operation _operation;
        private readonly ActivePrerequisite _activePrerequisite;

        public Modifier(Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null)
        {
            Priority = priority;
            _operation = operation;
            _activePrerequisite = activePrerequisite ?? (_ => true);
        }

        public float LastInvokeTime { get; private set; }

        public int InvokedCount { get; private set; }

        public int Priority { get; }

        public bool IsExpired { get; private set; }

        public virtual void Handle(object sender, Query query)
        {
            if (!IsExpired)
            {
                var contexts = new Contexts(sender, query, this);
                if (_activePrerequisite(contexts))
                {
                    _operation(contexts);
                    InvokedCount++;
                    LastInvokeTime = Time.time;
                    OnInvoke?.Invoke(this);
                }
                else
                {
                    OnInvokeFail?.Invoke(this);
                }
            }
        }

        public void Dispose()
        {
            IsExpired = true; // Might be redundant, but could be useful when calling Dispose manually. 
            OnDispose?.Invoke(this);
        }
    }

    public class Mediator
    {
        private readonly List<Modifier> _modifiers = new();

        public void PerformQuery(object sender, Query query)
        {
            foreach (var modifier in _modifiers.ToArray())
            {
                modifier.Handle(sender, query);
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

public class StatModifier : Stat.Modifier
{
    private readonly Stat.StatType _type;
    public StatModifier(Stat.StatType type, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
    {
        _type = type;
    }

    public override void Handle(object sender, Stat.Query query)
    {
        if (query.Stat.Type == _type)
        {
            base.Handle(sender, query);
        }
    }
}