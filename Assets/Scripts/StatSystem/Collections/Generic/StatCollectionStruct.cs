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

        public bool Contains(StatType type) => _stats.ContainsKey(type);

        public bool TryGetStat(StatType type, out T stat) => _stats.TryGetValue(type, out stat);

        IStat IReadOnlyStatCollection.this[StatType type] => this[type];

        IStat IStatCollection.this[StatType type]
        {
            get => this[type];
            set
            {
                if (value is T val)
                {
                    this[type] = val;
                }

                throw new Exception();
            }
        }

        public IEnumerable<StatType> Types => _stats.Keys;

        public IEnumerator<T> GetEnumerator() => _stats.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}