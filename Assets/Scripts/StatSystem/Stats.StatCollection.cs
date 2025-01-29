using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public partial class Stats
    {
        public class StatCollection : IStatCollection<Stat.MutableStat>, IStatCollection
        {
            private StatCollectionStruct<Stat.MutableStat> _statCollectionStruct;

            public StatCollection(IEnumerable<Stat.IStat> stats)
            {
                _statCollectionStruct = new StatCollectionStruct<Stat.MutableStat>(stats.Select(stat => new Stat.MutableStat(stat)));
            }

            public StatCollection(params Stat.IStat[] stats)
            {
                _statCollectionStruct = new StatCollectionStruct<Stat.MutableStat>(stats.Select(stat => new Stat.MutableStat(stat)));
            }

            public StatCollection(IStatCollection stats)
            {
                var mutableStats = stats.Cast<Stat.IStat>().Select(stat => new Stat.MutableStat(stat));
                _statCollectionStruct = new StatCollectionStruct<Stat.MutableStat>(mutableStats);
            }
        
            public Stat.MutableStat this[Stat.StatType type]
            {
                get => _statCollectionStruct[type];
                set => _statCollectionStruct[type] = value;
            }

            Stat.IStat IReadOnlyStatCollection.this[Stat.StatType type] => ((IReadOnlyStatCollection)_statCollectionStruct)[type];

            Stat.IStat IStatCollection.this[Stat.StatType type]
            {
                get => ((IStatCollection)_statCollectionStruct)[type];
                set => ((IStatCollection)_statCollectionStruct)[type] = value;
            }

            public IEnumerable<Stat.StatType> Types => _statCollectionStruct.Types;
            public IEnumerable<Stat.IStat> Values => _statCollectionStruct.Values;

            public bool Contains(Stat.StatType type) => _statCollectionStruct.Contains(type);

            public bool TryGetStat(Stat.StatType type, out Stat.MutableStat stat) => _statCollectionStruct.TryGetStat(type, out stat);

            public static implicit operator StatCollectionStruct<Stat>(StatCollection wrapper)
            {
                return new StatCollectionStruct<Stat>(wrapper._statCollectionStruct.Select(stat => (Stat)stat));
            }

            public IEnumerator<Stat.MutableStat> GetEnumerator() => _statCollectionStruct.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_statCollectionStruct).GetEnumerator();
        }
    }
}