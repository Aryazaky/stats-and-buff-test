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
            
            public StatCollection(IEnumerable<Stat> stats)
            {
                _statCollectionStruct = new StatCollectionStruct<Stat.MutableStat>(stats.Select(stat => new Stat.MutableStat(stat)));
            }
            
            public StatCollection(IEnumerable<Stat.MutableStat> stats)
            {
                _statCollectionStruct = new StatCollectionStruct<Stat.MutableStat>(stats);
            }

            public StatCollection(params Stat.IStat[] stats)
            {
                _statCollectionStruct = new StatCollectionStruct<Stat.MutableStat>(stats.Select(stat => new Stat.MutableStat(stat)));
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

            public bool Contains(Stat.StatType type) => _statCollectionStruct.Contains(type);
            public bool TryGetStat(Stat.StatType type, out Stat.IStat stat)
            {
                return _statCollectionStruct.TryGetStat(type, out stat);
            }

            public bool TryGetStat(Stat.StatType type, out Stat.MutableStat stat) => _statCollectionStruct.TryGetStat(type, out stat);

            public static implicit operator StatCollectionStruct<Stat>(StatCollection wrapper)
            {
                return new StatCollectionStruct<Stat>(wrapper._statCollectionStruct.Select<Stat.MutableStat, Stat>(stat => (Stat)stat));
            }

            IEnumerator<Stat.IStat> IEnumerable<Stat.IStat>.GetEnumerator()
            {
                return _statCollectionStruct.GetEnumerator();
            }

            public IEnumerator<Stat.MutableStat> GetEnumerator()
            {
                return _statCollectionStruct.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_statCollectionStruct).GetEnumerator();
        }
    }
}