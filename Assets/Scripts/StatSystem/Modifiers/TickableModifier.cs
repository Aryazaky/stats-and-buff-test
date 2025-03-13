using UnityEngine;

namespace StatSystem.Modifiers
{
    public class TickableModifier : Modifier, ITickable
    {
        private int _lastProcessedTick = -1;
        
        public TickableModifier(Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
        {
        }

        public void Tick()
        {
            TotalTicksElapsed++;
            LastTickTime = Time.time;
        }

        public int TotalTicksElapsed { get; private set; }
        public bool HasUnprocessedTick => TotalTicksElapsed > _lastProcessedTick;
        public float LastTickTime { get; private set; }
        public void MarkTickProcessed() => _lastProcessedTick = TotalTicksElapsed;

        protected override IModifierMetadata ExtractMetadata()
        {
            return new StatModifierMetadata(this);
        }
    }
}