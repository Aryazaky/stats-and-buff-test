using System;
using StatSystem.Collections;

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
        private readonly ITimeProvider _timeProvider;
        private readonly float _createdTime;
        private bool _isExpired;
        private bool _operationOnExpireTriggered;
        private int _lastProcessedTick = -1;

        public Modifier(Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null, ITimeProvider timeProvider = null)
        {
            Priority = priority;
            _operation = operation;
            _activePrerequisite = activePrerequisite ?? ((_,_) => true);
            _timeProvider = timeProvider ?? TimeProvider.Instance;
            _createdTime = _timeProvider.GetTime();
        }

        public float Age => _timeProvider.GetTime() - _createdTime;

        public int Priority { get; }

        public bool IsExpired { get; private set; }

        internal virtual void Handle(IQuery query)
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

        public void Tick()
        {
            TotalTicksElapsed++;
            LastTickTime = _timeProvider.GetTime();
        }

        public int TotalTicksElapsed { get; private set; }
        public bool HasUnprocessedTick => TotalTicksElapsed > _lastProcessedTick;
        public float LastTickTime { get; private set; }
        public void MarkTickProcessed(UpdateDetails updateDetails)
        {
            _lastProcessedTick = TotalTicksElapsed;
            LastUpdateDetails = updateDetails;
        }

        public UpdateDetails LastUpdateDetails { get; private set; }

        protected virtual IModifierMetadata ExtractMetadata()
        {
            return new ModifierMetadata(this);
        }

        public void Dispose()
        {
            IsExpired = true; // Might be redundant, but could be useful when calling Dispose manually. 
            OnDispose?.Invoke(this);
        }
    }
}