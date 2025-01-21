using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StatCollection
{
    public struct Stats<T> where T : Stat.IStat
    {
        private readonly Dictionary<Stat.StatType, T> _stats;

        public Stats(params T[] stats)
        {
            _stats = stats.ToDictionary(stat => stat.Type);
        }

        public Stats(IEnumerable<T> stats)
        {
            _stats = stats.ToDictionary(stat => stat.Type);
        }
    
        public T this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
        
        public bool Contains(Stat.StatType type)
        {
            return _stats.ContainsKey(type);
        }

        public IEnumerable<Stat.StatType> Types => _stats.Keys;
        public IEnumerable<T> Enumerable => _stats.Values;
    }

    public class ModifiableStats
    {
        private Stats<Stat.ModifiableStat> _stats;

        public ModifiableStats(Stats<Stat> stats)
        {
            _stats = new Stats<Stat.ModifiableStat>(stats.Enumerable.Select(stat => new Stat.ModifiableStat(stat)));
        }

        public ModifiableStats(StatCollection statCollection)
        {
            _stats = new Stats<Stat.ModifiableStat>(statCollection.Enumerable.Select(stat => new Stat.ModifiableStat(stat)));
        }
        
        public Stat.ModifiableStat this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
        
        public static implicit operator Stats<Stat>(ModifiableStats wrapper)
        {
            return new Stats<Stat>(wrapper._stats.Enumerable.Select(stat => (Stat)stat));
        }
    }

    public class BaseStatsIndexer
    {
        private readonly StatCollection _statCollection;
        public BaseStatsIndexer(StatCollection statCollection)
        {
            _statCollection = statCollection;
        }
        public Stat this[Stat.StatType type]
        {
            get => _statCollection._base[type];
            set => _statCollection._base[type] = value;
        }
    }

    private Stats<Stat> _base;
    private Stats<Stat> _modified;

    public Stat.Mediator Mediator { get; } = new();

    public StatCollection(params Stat[] stats)
    {
        _base = new Stats<Stat>(stats);
        _modified = new Stats<Stat>(stats);
    }
    
    public StatCollection(IEnumerable<Stat> stats)
    {
        _base = new Stats<Stat>(stats);
        _modified = new Stats<Stat>(stats);
    }

    public Stats<Stat> Base => _base;
    public Stats<Stat> Modified => _modified;
    
    public Stat this[Stat.StatType type] => _modified[type];

    public void Update(params Stat.StatType[] types)
    {
        if (!types.Any()) types = Types.ToArray();
        foreach (var type in types)
        {
            _modified[type] = PerformQuery(type); // This will make it so that the value of buffs be temporary. 
        }
    }

    private Stat PerformQuery(Stat.StatType type)
    {
        var query = new Stat.Query(this, type);
        Mediator.PerformQuery(new NoQueryArgs(this, query));
        return query.Stats[type];
    }

    private Stats<Stat> PerformQuery()
    {
        var query = new Stat.Query(this);
        Mediator.PerformQuery(new NoQueryArgs(this, query));
        return query.Stats;
    }
        
    public bool Contains(Stat.StatType type)
    {
        return _base.Contains(type);
    }

    public IEnumerable<Stat.StatType> Types => _base.Types;
    public IEnumerable<Stat> Enumerable => _base.Enumerable;
}