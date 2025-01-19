using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public readonly partial struct Stat
{
    public abstract class Modifier : IDisposable
    {
        public readonly struct Contexts
        {
            public readonly object sender;
            public readonly Query query;
            public readonly Modifier modifier;

            public Contexts(object sender, Query query, Modifier modifier)
            {
                this.sender = sender;
                this.query = query;
                this.modifier = modifier;
            }
        }

        public delegate void Operation(Contexts contexts);
        public delegate bool ActivePrerequisite(Contexts contexts); // To allow enable or disable without expiring the modifier
        public event Action<Modifier> OnDispose;
        public event Action<Modifier> OnInvoke;
        public event Action<Modifier> OnInvokeFail;

        private readonly Operation operation;
        private readonly ActivePrerequisite activePrerequisite;

        public Modifier(Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null)
        {
            Priority = priority;
            this.operation = operation;
            this.activePrerequisite = activePrerequisite ?? (contexts => true);
        }

        public float LastInvokeTime { get; private set; }

        public int InvokedCount { get; private set; }

        public int Priority { get; }

        public bool IsExpired { get; private set; }

        public virtual void Handle(object sender, Query query)
        {
            if (!IsExpired)
            {
                Contexts contexts = new Contexts(sender, query, this);
                if (activePrerequisite(contexts))
                {
                    operation(contexts);
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
        private readonly List<Modifier> modifiers = new();

        public void PerformQuery(object sender, Query query)
        {
            foreach (var modifier in modifiers.ToArray())
            {
                modifier.Handle(sender, query);
            }

            RemoveModifiers(mod => mod.IsExpired);
        }

        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
            modifiers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            modifier.OnDispose += mod => modifiers.Remove(mod);
        }

        public void RemoveModifiers(Func<Modifier, bool> predicate)
        {
            foreach (var modifier in modifiers.Where(predicate).ToArray())
            {
                modifier.Dispose();
            }
        }
    }
}

public class StatModifier : Stat.Modifier
{
    private Stat.StatType type;
    public StatModifier(Stat.StatType type, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
    {
        this.type = type;
    }

    public override void Handle(object sender, Stat.Query query)
    {
        if (query.type == type)
        {
            base.Handle(sender, query);
        }
    }
}