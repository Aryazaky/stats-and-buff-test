using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem.Collections
{
    public readonly struct StatCollectionStruct : IStatCollection, IEnumerable<Stat>
    {
        private readonly Dictionary<StatType, Stat> _stats;

        public StatCollectionStruct(params Stat[] stats) => _stats = stats.ToDictionary(stat => stat.Type);

        public StatCollectionStruct(IEnumerable<Stat> stats) => _stats = stats.ToDictionary(stat => stat.Type);

        public Stat this[StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }

        public bool Contains(params StatType[] type)
        {
            var s = _stats;
            return type.All(t => s.ContainsKey(t));
        }

        public bool TryGetStat(StatType type, out Stat stat) => _stats.TryGetValue(type, out stat);

        public IEnumerable<StatType> Types => _stats.Keys;

        public IEnumerator<Stat> GetEnumerator() => _stats.Values.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return string.Join(", ", _stats.Values);
        }
    }
}