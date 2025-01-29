using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public partial class Stats
    {
        public struct StatCollectionStruct<T> : IStatCollection<T>, IStatCollection where T : Stat.IStat
        {
            private readonly Dictionary<Stat.StatType, T> _stats;

            public StatCollectionStruct(params T[] stats) => _stats = stats.ToDictionary(stat => stat.Type);

            public StatCollectionStruct(IEnumerable<T> stats) => _stats = stats.ToDictionary(stat => stat.Type);

            public T this[Stat.StatType type]
            {
                get => _stats[type];
                set => _stats[type] = value;
            }

            public IEnumerable<Stat.IStat> Values => _stats.Values.Cast<Stat.IStat>();

            public bool Contains(Stat.StatType type) => _stats.ContainsKey(type);

            public bool TryGetStat(Stat.StatType type, out T stat) => _stats.TryGetValue(type, out stat);

            Stat.IStat IReadOnlyStatCollection.this[Stat.StatType type] => this[type];

            Stat.IStat IStatCollection.this[Stat.StatType type]
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

            public IEnumerable<Stat.StatType> Types => _stats.Keys;

            public IEnumerator<T> GetEnumerator() => _stats.Values.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}