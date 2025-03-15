using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatSystem.Modifiers;

namespace StatSystem.Collections
{
    public class Stats : IEnumerable<MutableStat>, IMutableStatCollection
    {
        private readonly StatCollection _base;
        private readonly StatCollection _modified;

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
        public StatCollectionStruct Snapshot() => _modified;

        public void Bake()
        {
            foreach (var type in _base.Types.ToArray())
            {
                _base[type] = _modified[type];
            }
        }
        
        public void Update(IReadOnlyWorldContexts worldContexts)
        {
            foreach (var tickable in Mediator.OfType<ITickable>()) tickable.Tick();
            var temp = PerformQuery(worldContexts);
            foreach (var stat in temp)
            {
                _modified[stat.Type] = stat;
            }

            IsDirty = false;
        }

        private StatCollection PerformQuery(IReadOnlyWorldContexts worldContexts)
        {
            var query = new Query(_base, worldContexts);
            Mediator.PerformQuery(query);
            return query.QueriedStats;
        }

        public IEnumerable<StatType> Types => _base.Types;

        public MutableStat this[StatType type]
        {
            get => _modified[type];
            set
            {
                var prev = _modified[type].Value;
                var diff = value.Value - prev;
                _modified[type] = value;
                _base[type] += diff;
            }
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