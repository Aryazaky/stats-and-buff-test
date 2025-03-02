using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Modifiers;

namespace StatSystem.Collections
{
    [Serializable]
    public class Stats : IEnumerable<MutableStat>
    {
        private StatCollection _base;
        private StatCollection _modified;

        public Stats(params Stat[] stats)
        {
            _base = new StatCollection(stats);
            _modified = new StatCollection(stats);
            Mediator = new Mediator(_ => IsDirty = true, _ => IsDirty = true);
        }
    
        public Stats(IEnumerable<Stat> stats)
        {
            _base = new StatCollection(stats);
            _modified = new StatCollection(stats);
            Mediator = new Mediator(_ => IsDirty = true, _ => IsDirty = true);
        }

        public Mediator Mediator { get; }
        public bool IsDirty { get; private set; }

        public IEnumerable<StatType> Types => _base.Types;

        public MutableStat this[StatType type]
        {
            get => _modified[type];
            set
            {
                var prev = _modified[type];
                var diff = value.Value - prev.Value;
                _modified[type] = value;
                _base[type] += diff;
            }
        }

        public void Bake()
        {
            foreach (var type in _base.Types.ToArray())
            {
                _base[type] = _modified[type];
            }
        }
        
        public void Update(IReadOnlyWorldContexts worldContexts, params StatType[] types)
        {
            var temp = PerformQuery(worldContexts);
            if (!types.Any())
            {
                _modified = temp;
            }
            else foreach (var type in types)
            {
                _modified[type] = temp[type];
            }

            IsDirty = false;
        }

        private StatCollection PerformQuery(IReadOnlyWorldContexts worldContexts)
        {
            var query = new Query(_base, worldContexts, _base.Types.ToArray());
            Mediator.PerformQuery(query);
            return query.TemporaryStats;
        }
        
        public bool Contains(params StatType[] type) => _base.Contains(type);

        public bool TryGetStat(StatType type, out MutableStat stat) => _modified.TryGetStat(type, out stat);

        public IEnumerator<MutableStat> GetEnumerator() => _modified.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            return _modified.ToString();
        }

    }
}