using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    [Serializable]
    public partial class Stats : Stats.IStatCollection<Stat>, Stats.IStatCollection
    {
        private StatCollectionStruct<Stat> _base;
        private StatCollectionStruct<Stat> _modified;

        public Stat.Mediator Mediator { get; } = new();

        public Stats(params Stat[] stats)
        {
            _base = new StatCollectionStruct<Stat>(stats);
            _modified = new StatCollectionStruct<Stat>(stats);
        }
    
        public Stats(IEnumerable<Stat> stats)
        {
            _base = new StatCollectionStruct<Stat>(stats);
            _modified = new StatCollectionStruct<Stat>(stats);
        }

        public StatCollectionStruct<Stat> Base => _base;
        public StatCollectionStruct<Stat> Modified => _modified;

        public Stat this[Stat.StatType type]
        {
            get => _modified[type];
            set => _base[type] = value;
        }

        public void Update(WorldContexts worldContexts, params Stat.StatType[] types)
        {
            if (!types.Any()) types = Types.ToArray();
            foreach (var type in types)
            {
                _modified[type] = PerformQuery(type, worldContexts); // This will make it so that the value of buffs be temporary. 
            }
        }

        private Stat PerformQuery(Stat.StatType type, WorldContexts worldContexts)
        {
            var query = new Stat.Query(this, worldContexts, type);
            Mediator.PerformQuery(query);
            return query.Stats[type];
        }

        private StatCollectionStruct<Stat> PerformQuery(WorldContexts worldContexts)
        {
            var query = new Stat.Query(this, worldContexts);
            Mediator.PerformQuery(query);
            return query.Stats;
        }

        public IEnumerable<Stat.IStat> Values => _base.Values;

        public bool Contains(Stat.StatType type)
        {
            return _base.Contains(type);
        }

        public bool TryGetStat(Stat.StatType type, out Stat stat) => _base.TryGetStat(type, out stat);

        Stat.IStat IReadOnlyStatCollection.this[Stat.StatType type] => ((IReadOnlyStatCollection)_base)[type];

        Stat.IStat IStatCollection.this[Stat.StatType type]
        {
            get => ((IStatCollection)_base)[type];
            set => ((IStatCollection)_base)[type] = value;
        }

        public IEnumerable<Stat.StatType> Types => _base.Types;

        public IEnumerator<Stat> GetEnumerator() => _base.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}