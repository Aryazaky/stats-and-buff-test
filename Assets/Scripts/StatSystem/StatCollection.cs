using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public class StatCollection : IStatCollection<MutableStat>, IStatCollection
    {
        private StatCollectionStruct<MutableStat> _statCollectionStruct;

        public StatCollection(IEnumerable<IStat> stats)
        {
            _statCollectionStruct = new StatCollectionStruct<MutableStat>(stats.Select(stat => new MutableStat(stat)));
        }

        public StatCollection(params IStat[] stats)
        {
            _statCollectionStruct = new StatCollectionStruct<MutableStat>(stats.Select(stat => new MutableStat(stat)));
        }

        public StatCollection(IStatCollection stats)
        {
            var mutableStats = stats.Cast<IStat>().Select(stat => new MutableStat(stat));
            _statCollectionStruct = new StatCollectionStruct<MutableStat>(mutableStats);
        }
        
        public MutableStat this[Stat.StatType type]
        {
            get => _statCollectionStruct[type];
            set => _statCollectionStruct[type] = value;
        }

        IStat IReadOnlyStatCollection.this[Stat.StatType type] => ((IReadOnlyStatCollection)_statCollectionStruct)[type];

        IStat IStatCollection.this[Stat.StatType type]
        {
            get => ((IStatCollection)_statCollectionStruct)[type];
            set => ((IStatCollection)_statCollectionStruct)[type] = value;
        }

        public IEnumerable<Stat.StatType> Types => _statCollectionStruct.Types;

        public bool Contains(Stat.StatType type) => _statCollectionStruct.Contains(type);

        public bool TryGetStat(Stat.StatType type, out MutableStat stat) => _statCollectionStruct.TryGetStat(type, out stat);

        public static implicit operator StatCollectionStruct<Stat>(StatCollection wrapper)
        {
            return new StatCollectionStruct<Stat>(wrapper._statCollectionStruct.Select(stat => (Stat)stat));
        }

        public IEnumerator<MutableStat> GetEnumerator() => _statCollectionStruct.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_statCollectionStruct).GetEnumerator();
    }
}