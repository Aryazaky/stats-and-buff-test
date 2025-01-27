using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public partial class Stats
    {
        public class ModifiableStatDictionary : IReadOnlyDictionary<Stat.StatType, Stat.ModifiableStat>
        {
            private StatDictionary<Stat.ModifiableStat> _statDictionary;

            public ModifiableStatDictionary(StatDictionary<Stat> statDictionary)
            {
                _statDictionary = new StatDictionary<Stat.ModifiableStat>(statDictionary.Select(stat => new Stat.ModifiableStat(stat.Value)));
            }

            public ModifiableStatDictionary(Stats stats)
            {
                _statDictionary = new StatDictionary<Stat.ModifiableStat>(stats.Select(stat => new Stat.ModifiableStat(stat)));
            }
        
            public Stat.ModifiableStat this[Stat.StatType type]
            {
                get => _statDictionary[type];
                set => _statDictionary[type] = value;
            }

            public IEnumerable<Stat.StatType> Keys => _statDictionary.Keys;

            public IEnumerable<Stat.ModifiableStat> Values => _statDictionary.Values;

            public bool ContainsKey(Stat.StatType type) => _statDictionary.ContainsKey(type);

            public bool TryGetValue(Stat.StatType type, out Stat.ModifiableStat stat) => _statDictionary.TryGetValue(type, out stat);

            public static implicit operator StatDictionary<Stat>(ModifiableStatDictionary wrapper)
            {
                return new StatDictionary<Stat>(wrapper._statDictionary.Select(keyValuePair => (Stat)keyValuePair.Value));
            }

            public IEnumerator<KeyValuePair<Stat.StatType, Stat.ModifiableStat>> GetEnumerator()
            {
                return _statDictionary.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_statDictionary).GetEnumerator();

            public int Count => _statDictionary.Count;
        }
    }
}