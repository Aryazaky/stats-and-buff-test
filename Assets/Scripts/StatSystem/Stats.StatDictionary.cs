using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public partial class Stats
    {
        public struct StatDictionary<T> : IReadOnlyDictionary<Stat.StatType, T> where T : Stat.IStat
        {
            private readonly Dictionary<Stat.StatType, T> _stats;

            public StatDictionary(params T[] stats) => _stats = stats.ToDictionary(stat => stat.Type);

            public StatDictionary(IEnumerable<T> stats) => _stats = stats.ToDictionary(stat => stat.Type);

            public T this[Stat.StatType type]
            {
                get => _stats[type];
                set => _stats[type] = value;
            }
        
            public bool ContainsKey(Stat.StatType type) => _stats.ContainsKey(type);
            public bool TryGetValue(Stat.StatType type, out T stat) => _stats.TryGetValue(type, out stat);
            public IEnumerable<Stat.StatType> Keys => _stats.Keys;
            public IEnumerable<T> Values => _stats.Values;
            public IEnumerator<KeyValuePair<Stat.StatType, T>> GetEnumerator() => _stats.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public int Count => _stats.Count;
        }
    }
}