﻿using System;
using System.Collections.Generic;
using System.Linq;

public readonly partial struct Stat
{
    public abstract class Modifier : IDisposable
    {
        public interface IExpiryNotifier // To end this once and for all
        {
            event Action OnExpired;
        }

        public delegate void Operation(object sender, Query query);
        public delegate bool ActivePrerequisite(object sender, Query query); // To allow enable or disable without expiring the modifier
        public event Action<Modifier> OnDispose;

        private int priority;
        private Operation operation;
        private ActivePrerequisite activePrerequisite;
        public int InvokedCount { get; private set; }

        public Modifier(int priority, Operation operation, ActivePrerequisite activePrerequisite = null, IExpiryNotifier expiryNotifier = null)
        {
            this.priority = priority;
            this.operation = operation;
            if (expiryNotifier != null)
            {
                expiryNotifier.OnExpired += () => IsExpired = true;
            }
            if (activePrerequisite != null)
            {
                this.activePrerequisite = activePrerequisite;
            }
            else this.activePrerequisite = (sender, query) => true;
        }

        public int Priority => priority;

        public bool IsExpired { get; private set; }

        public virtual void Handle(object sender, Query query)
        {
            if (activePrerequisite(sender, query) && !IsExpired)
            {
                operation(sender, query);
                InvokedCount++;
            }
        }

        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }
    }

    public class Modifiers
    {
        private readonly List<Modifier> modifiers = new();

        public void PerformQuery(object sender, Query query)
        {
            foreach (var modifier in modifiers)
            {
                modifier.Handle(sender, query);
            }

            foreach (var modifier in modifiers.Where(mod => mod.IsExpired))
            {
                modifier.Dispose();
            }
        }

        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
            modifiers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            modifier.OnDispose += mod => modifiers.Remove(mod);
        }
    }
}

public class StatModifier : Stat.Modifier
{
    private Stat.StatType type;
    public StatModifier(Stat.StatType type, int priority, Operation operation, ActivePrerequisite activePrerequisite = null, IExpiryNotifier expiryNotifier = null) : base(priority, operation, activePrerequisite, expiryNotifier)
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