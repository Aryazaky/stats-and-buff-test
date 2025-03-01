using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Collections.Generic;

namespace StatSystem.Collections
{
    public class StatCollection : IStatCollection
    {
        private StatCollectionStruct<IStat> _statCollection;

        public StatCollection(IEnumerable<IStat> stats)
        {
            _statCollection = new StatCollectionStruct<IStat>(stats.Select(stat => new MutableStat(stat)));
        }

        public StatCollection(params IStat[] stats)
        {
            _statCollection = new StatCollectionStruct<IStat>(stats.Select(stat => new MutableStat(stat)));
        }

        public StatCollection(IStatCollection stats)
        {
            var mutableStats = stats.Cast<IStat>().Select(stat => new MutableStat(stat));
            _statCollection = new StatCollectionStruct<IStat>(mutableStats);
        }
        
        public IStat this[StatType type]
        {
            get => _statCollection[type];
            set => _statCollection[type] = value.ConvertTo<MutableStat>();
        }

        public IEnumerable<StatType> Types => _statCollection.Types;

        public bool Contains(params StatType[] type) => _statCollection.Contains(type);

        public bool TryGetStat(StatType type, out IStat stat) => _statCollection.TryGetStat(type, out stat);

        public static implicit operator StatCollectionStruct<Stat>(StatCollection wrapper)
        {
            return new StatCollectionStruct<Stat>(wrapper._statCollection.Select(stat => (Stat)stat));
        }

        public IEnumerator<IStat> GetEnumerator() => _statCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_statCollection).GetEnumerator();
    }
}