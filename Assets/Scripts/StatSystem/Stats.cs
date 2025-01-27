using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    [Serializable]
    public partial class Stats : IEnumerable<Stat>
    {
        private StatDictionary<Stat> _base;
        private StatDictionary<Stat> _modified;

        public Stat.Mediator Mediator { get; } = new();

        public Stats(params Stat[] stats)
        {
            _base = new StatDictionary<Stat>(stats);
            _modified = new StatDictionary<Stat>(stats);
        }
    
        public Stats(IEnumerable<Stat> stats)
        {
            _base = new StatDictionary<Stat>(stats);
            _modified = new StatDictionary<Stat>(stats);
        }

        public StatDictionary<Stat> Base => _base;
        public StatDictionary<Stat> Modified => _modified;
    
        public Stat this[Stat.StatType type] => _modified[type];

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
            return query.ModifiableStats[type];
        }

        private StatDictionary<Stat> PerformQuery(WorldContexts worldContexts)
        {
            var query = new Stat.Query(this, worldContexts);
            Mediator.PerformQuery(query);
            return query.ModifiableStats;
        }
        
        public bool Contains(Stat.StatType type)
        {
            return _base.ContainsKey(type);
        }

        public IEnumerable<Stat.StatType> Types => _base.Keys;
        public IEnumerator<Stat> GetEnumerator()
        {
            return _base.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}