using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Modifiers;

namespace StatSystem.Collections.Generic
{
    public class Stats<T> : IStatCollection<T>, IStatCollection where T : IStat
    {
        private StatCollectionStruct<T> _base;
        private StatCollectionStruct<T> _modified;

        public Stats(params T[] stats)
        {
            _base = new StatCollectionStruct<T>(stats);
            _modified = new StatCollectionStruct<T>(stats);
        }
    
        public Stats(IEnumerable<T> stats)
        {
            _base = new StatCollectionStruct<T>(stats);
            _modified = new StatCollectionStruct<T>(stats);
        }

        public Mediator Mediator { get; } = new();

        public StatCollectionStruct<T> Base => _base;

        public StatCollectionStruct<T> Modified => _modified;
        
        public IEnumerable<StatType> Types => _base.Types;

        public T this[StatType type]
        {
            get => _modified[type];
            set => _base[type] = value;
        }
        
        public bool Contains(StatType type)
        {
            return _base.Contains(type);
        }

        public bool TryGetStat(StatType type, out T stat) => _modified.TryGetStat(type, out stat);

        IStat IReadOnlyStatCollection.this[StatType type] => _modified[type];

        IStat IStatCollection.this[StatType type]
        {
            get => _modified[type];
            set => _base[type] = value.ConvertTo<T>();
        }
        
        public void Update(WorldContexts worldContexts, params StatType[] types)
        {
            if (!types.Any()) types = Types.ToArray();
            foreach (var type in types)
            {
                _modified[type] = PerformQuery(type, worldContexts); // This will make it so that the value of buffs be temporary. 
            }
        }

        private T PerformQuery(StatType type, WorldContexts worldContexts)
        {
            var query = new Query(this, worldContexts, type);
            Mediator.PerformQuery(query);
            var queryStat = query.Stats[type];
            if (queryStat is T stat)
            {
                return stat;
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T), queryStat.Type, queryStat.Value, queryStat.Min, queryStat.Max, queryStat.Precision);
            }
        }

        private StatCollectionStruct<T> PerformQuery(WorldContexts worldContexts)
        {
            var query = new Query(this, worldContexts);
            Mediator.PerformQuery(query);
            return new StatCollectionStruct<T>(query.Stats.Cast<T>());
        }

        public IEnumerator<T> GetEnumerator() => _modified.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}