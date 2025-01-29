using System;
using UnityEngine;

namespace StatSystem
{
    public readonly partial struct Stat
    {
        public abstract partial class Modifier : IDisposable, Modifier.IModifier
        {
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

            public virtual void Handle(StatQuery query)
            {
                if (!IsExpired)
                {
                    OnTryInvoke?.Invoke(this);
                    var contexts = new Contexts(query, this);
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
    }
}