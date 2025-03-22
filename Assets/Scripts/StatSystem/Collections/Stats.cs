using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Modifiers;

namespace StatSystem.Collections
{
    public class Stats : IEnumerable<MutableStat>, IMutableStatCollection
    {
        private readonly StatCollection _base;
        private readonly StatCollection _modified;

        public Stats(Mediator mediator = null, params Stat[] stats) : this(stats, mediator)
        {
        }
    
        public Stats(IEnumerable<Stat> stats, Mediator mediator = null)
        {
            _base = new StatCollection(stats);
            _modified = new StatCollection(stats);
            Mediator = mediator ?? new Mediator(_ => IsDirty = true, _ => IsDirty = true);
        }

        public Mediator Mediator { get; }
        public bool IsDirty { get; private set; }
        public StatCollectionStruct Snapshot() => _modified;

        public void Bake()
        {
            foreach (var type in _base.Types.ToArray())
            {
                _base[type] = _modified[type];
            }
        }
        
        public void Update(IReadOnlyWorldContexts worldContexts)
        {
            foreach (var tickable in Mediator) tickable.Tick();
            var temp = PerformQuery(worldContexts);
            foreach (var stat in temp)
            {
                _modified[stat.Type] = stat;
            }

            IsDirty = false;
        }

        private StatCollection PerformQuery(IReadOnlyWorldContexts worldContexts)
        {
            var query = new Query(_base, worldContexts);
            Mediator.PerformQuery(query);
            return query.QueriedStats;
        }

        public IEnumerable<StatType> Types => _base.Types;

        public MutableStat this[StatType type]
        {
            get => _modified[type];
            set
            {
                if (!Contains(type))
                {
                    throw new ArgumentException($"StatType {type} does not exist in this collection.");
                }

                var modifiedStat = _modified[type];
                var baseStat = _base[type];

                // Store previous values
                var prevValue = modifiedStat.Value;
                var prevMin = modifiedStat.Min;
                var prevMax = modifiedStat.Max;

                // Set
                _modified[type] = value;

                // Compute differences
                var diffValue = modifiedStat.Value - prevValue;
                var diffMin = (prevMin.HasValue && modifiedStat.Min.HasValue) ? modifiedStat.Min.Value - prevMin.Value : (float?)null;
                var diffMax = (prevMax.HasValue && modifiedStat.Max.HasValue) ? modifiedStat.Max.Value - prevMax.Value : (float?)null;

                // Adjust base stat
                baseStat.Value += diffValue;
                baseStat.Min = diffMin.HasValue ? (baseStat.Min ?? 0) + diffMin.Value : modifiedStat.Min;
                baseStat.Max = diffMax.HasValue ? (baseStat.Max ?? 0) + diffMax.Value : modifiedStat.Max;
            }
        }
        
        public bool Contains(params StatType[] type) => _base.Contains(type);

        public bool TryGetStat(StatType type, out MutableStat stat) => _modified.TryGetStat(type, out stat);
        public bool SafeEdit(StatType type, Action<MutableStat> editAction)
        {
            if (!Contains(type))
            {
                return false;
            }

            var modifiedStat = _modified[type];
            var baseStat = _base[type];

            // Store previous values
            var prevValue = modifiedStat.Value;
            var prevMin = modifiedStat.Min;
            var prevMax = modifiedStat.Max;

            // Apply the edit
            editAction(modifiedStat);

            // Compute differences
            var diffValue = modifiedStat.Value - prevValue;
            var diffMin = (prevMin.HasValue && modifiedStat.Min.HasValue) ? modifiedStat.Min.Value - prevMin.Value : (float?)null;
            var diffMax = (prevMax.HasValue && modifiedStat.Max.HasValue) ? modifiedStat.Max.Value - prevMax.Value : (float?)null;

            // Adjust base stat
            baseStat.Value += diffValue;
            baseStat.Min = diffMin.HasValue ? (baseStat.Min ?? 0) + diffMin.Value : modifiedStat.Min;
            baseStat.Max = diffMax.HasValue ? (baseStat.Max ?? 0) + diffMax.Value : modifiedStat.Max;

            return true;
        }

        public IEnumerator<MutableStat> GetEnumerator() => _modified.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return _modified.ToString();
        }
    }
}