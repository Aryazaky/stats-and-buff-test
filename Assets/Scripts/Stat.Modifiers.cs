using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public readonly partial struct Stat
{
    public abstract class Modifier : IDisposable, Modifier.IModifier
    {
        public static class PriorityType
        {
            public const int Default = 0;
            public const int Boost = 1; // Equipment stat boosts and such. They're additive BUT done before multiplication to simulate as if the base stats were always like that. 
            public const int Multiplicative = 2; // Buffs or debuffs
            public const int Offset = 3; // This is also additive, but done normally. For flat stat increase/decrease unaffected by buffs
            public const int Override = 4; // Evil, the ultimate No U. Could set the hard work we all did to calculate all that to 0 for all I know. 
            public const int Clamp = 5;
        }
        
        private interface IModifier
        {
            public float LastInvokeTime { get; }
            public float CreatedTime { get; }
            public int InvokedCount { get; }
            public int Priority { get; }
        }
        
        public interface IExpiryNotifier
        {
            void TrackModifier(Modifier modifier);
            void CheckLimit();
        }
        
        public readonly struct Metadata : IModifier
        {
            private readonly Modifier _modifier;
            public Metadata(Modifier modifier)
            {
                _modifier = modifier;
            }

            public float LastInvokeTime => _modifier.LastInvokeTime;
            public float CreatedTime => _modifier.CreatedTime;
            public int InvokedCount => _modifier.InvokedCount;
            public int Priority => _modifier.Priority;
        }

        public readonly struct Contexts
        {
            public readonly QueryArgs QueryArgs;
            public readonly Metadata ModifierMetadata;

            public Contexts(QueryArgs queryArgs, Modifier modifier)
            {
                QueryArgs = queryArgs;
                ModifierMetadata = new Metadata(modifier);
            }
        }

        public interface IExpireTrigger
        {
            void Expire();
        }

        private class ExpireTrigger : IExpireTrigger
        {
            private readonly Modifier _modifier;

            public ExpireTrigger(Modifier modifier)
            {
                _modifier = modifier;
            }

            public void Expire()
            {
                _modifier.IsExpired = true;
            }
        }

        public delegate void Operation(Contexts contexts);
        public delegate bool ActivePrerequisite(Contexts contexts, IExpireTrigger trigger);
        public event Action<Modifier> OnTryInvoke;
        public event Action<Modifier> OnInvoke;
        public event Action<Modifier> OnInvokeFail;
        public event Action<Modifier> OnDispose;

        private readonly Operation _operation;
        private readonly ActivePrerequisite _activePrerequisite;
        private bool _isExpired;
        private bool _operationOnExpireTriggered;

        public Modifier(Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null)
        {
            Priority = priority;
            _operation = operation;
            _activePrerequisite = activePrerequisite ?? ((_,_) => true);
            CreatedTime = Time.time;
        }

        public float LastInvokeTime { get; private set; }
        
        public float CreatedTime { get; }

        public int InvokedCount { get; private set; }

        public int Priority { get; }

        public bool IsExpired { get; private set; }

        public virtual void Handle(QueryArgs queryArgs)
        {
            if (!IsExpired)
            {
                OnTryInvoke?.Invoke(this);
                var contexts = new Contexts(queryArgs, this);
                if (_activePrerequisite(contexts, new ExpireTrigger(this)))
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

public class StatModifier : Stat.Modifier
{
    private readonly Stat.StatType[] _types;
    public StatModifier(IEnumerable<Stat.StatType> types, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
    {
        _types = types.ToArray();
    }
    public StatModifier(Stat.StatType type, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
    {
        _types = new[] { type };
    }

    public override void Handle(QueryArgs queryArgs)
    {
        if (queryArgs.Query.Types.Intersect(_types).Any())
        {
            base.Handle(queryArgs);
        }
    }
}