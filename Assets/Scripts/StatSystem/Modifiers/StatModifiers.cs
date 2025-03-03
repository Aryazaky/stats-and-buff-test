using System.Collections.Generic;
using System.Linq;
using StatSystem.Collections;
using UnityEngine;

namespace StatSystem.Modifiers
{
    public class StatModifier : Modifier, ITickable
    {
        private readonly StatType[] _types;
        private int _lastProcessedTick = -1;
        public StatModifier(IEnumerable<StatType> types, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
        {
            _types = types.ToArray();
        }
        public StatModifier(StatType type, Operation operation, int priority = 0, ActivePrerequisite activePrerequisite = null) : base(operation, priority, activePrerequisite)
        {
            _types = new[] { type };
        }

        public override void Handle(IQuery query)
        {
            if (query.Types.Intersect(_types).Any())
            {
                base.Handle(query);
            }
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

        protected override IModifier ExtractMetadata()
        {
            return new StatMetadata(this);
        }
    }
}