using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem.Collections.Generic
{
    public struct StatCollectionStruct<T> : IStatCollection<T>, IStatCollection where T : IStat
    {
        private readonly Dictionary<StatType, T> _stats;

        public StatCollectionStruct(params T[] stats) => _stats = stats.ToDictionary(stat => stat.Type);

        public StatCollectionStruct(IEnumerable<T> stats) => _stats = stats.ToDictionary(stat => stat.Type);

        public T this[StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
        
        IStat IIndexer.this[StatType type]
        {
            get => this[type];
            set => this[type] = value.ConvertTo<T>();
        }

        public bool Contains(params StatType[] type)
        {
            var s = _stats;
            return type.All(t => s.ContainsKey(t));
        }

        public bool TryGetStat(StatType type, out IStat stat)
        {
            if (_stats.TryGetValue(type, out var value))
            {
                stat = value;
                return true;
            }

            stat = null;
            return false;
        }

        public bool TryGetStat(StatType type, out T stat) => _stats.TryGetValue(type, out stat);

        public IEnumerable<StatType> Types => _stats.Keys;

        public IEnumerator<T> GetEnumerator() => _stats.Values.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return string.Join(", ", _stats.Values);
        }
    }
}