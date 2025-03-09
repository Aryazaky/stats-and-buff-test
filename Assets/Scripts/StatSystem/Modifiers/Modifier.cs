using System;
using StatSystem.Collections;
using UnityEngine;

namespace StatSystem.Modifiers
{
    public partial class Modifier : IDisposable, Modifier.IModifierMetadata
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
        private readonly float _createdTime;

        public Modifier(Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null)
        {
            Priority = priority;
            _operation = operation;
            _activePrerequisite = activePrerequisite ?? ((_,_) => true);
            _createdTime = Time.time;
        }

        public float Age => Time.time - _createdTime;

        public int Priority { get; }

        public bool IsExpired { get; private set; }

        public virtual void Handle(IQuery query)
        {
            if (!IsExpired)
            {
                OnTryInvoke?.Invoke(this);
                var contexts = new Contexts(query, this);
                if (_activePrerequisite(contexts, new ExpireTrigger(this)))
                {
                    _operation(contexts);
                    OnInvoke?.Invoke(this);
                }
                else
                {
                    OnInvokeFail?.Invoke(this);
                }
            }
        }
        
        protected virtual IModifierMetadata ExtractMetadata()
        {
            return new Metadata(this);
        }

        public void Dispose()
        {
            IsExpired = true; // Might be redundant, but could be useful when calling Dispose manually. 
            OnDispose?.Invoke(this);
        }
    }
}