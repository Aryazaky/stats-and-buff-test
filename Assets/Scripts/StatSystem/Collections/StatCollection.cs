using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem.Collections
{
    public class StatCollection : IEnumerable<MutableStat>, IReadOnlyStatCollection
    {
        private readonly Dictionary<StatType, MutableStat> _stats;

        public StatCollection(params Stat[] stats) => _stats = stats.Select(stat => new MutableStat(stat)).ToDictionary(stat => stat.Type);

        public StatCollection(IEnumerable<Stat> stats) => _stats = stats.Select(stat => new MutableStat(stat)).ToDictionary(stat => stat.Type);

        public MutableStat this[StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }

        public bool Contains(params StatType[] type)
        {
            var s = _stats;
            return type.All(t => s.ContainsKey(t));
        }

        public bool TryGetStat(StatType type, out MutableStat stat) => _stats.TryGetValue(type, out stat);

        public IEnumerable<StatType> Types => _stats.Keys;

        public IEnumerator<MutableStat> GetEnumerator() => _stats.Values.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return string.Join(", ", _stats.Values);
        }
        
        public static implicit operator StatCollectionStruct(StatCollection statCollection)
        {
            return new StatCollectionStruct(statCollection.Select(stat => new Stat(stat.Type, stat.Value, stat.Min, stat.Max, stat.Precision)));
        }
    }
}