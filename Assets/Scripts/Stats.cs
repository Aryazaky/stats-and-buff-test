using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stats
{
    public struct Data<T> where T : IStat
    {
        private readonly Dictionary<Stat.StatType, T> _stats;

        public Data(params T[] stats)
        {
            _stats = stats.ToDictionary(stat => stat.Type);
        }

        public Data(IEnumerable<T> stats)
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

    public class DataWrapper
    {
        private Data<Stat.MutableStat> _stats;

        public DataWrapper(Data<Stat> data)
        {
            _stats = new Data<Stat.MutableStat>(data.Enumerable.Select(stat => new Stat.MutableStat(stat)));
        }

        public DataWrapper(Stats stats)
        {
            _stats = new Data<Stat.MutableStat>(stats.Enumerable.Select(stat => new Stat.MutableStat(stat)));
        }
        
        public Stat.MutableStat this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
        
        public static implicit operator Data<Stat>(DataWrapper wrapper)
        {
            return new Data<Stat>(wrapper._stats.Enumerable.Select(stat => (Stat)stat));
        }
    }

    public class IndexerOnly
    {
        private readonly Stats _stats;
        public IndexerOnly(Stats stats)
        {
            _stats = stats;
        }
        public Stat this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
    }

    private Data<Stat> _base;
    private Data<Stat> _modified;

    public Stat.Mediator Mediator { get; } = new();

    public Stats(params Stat[] stats)
    {
        _base = new Data<Stat>(stats);
        _modified = new Data<Stat>(stats);
    }
    
    public Stats(IEnumerable<Stat> stats)
    {
        _base = new Data<Stat>(stats);
        _modified = new Data<Stat>(stats);
    }

    public Stat this[Stat.StatType type]
    {
        get => _modified[type];
        set => _base[type] = value;
    }

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
        Mediator.PerformQuery(this, query);
        return query.Stats[type];
    }

    private Data<Stat> PerformQuery()
    {
        var query = new Stat.Query(this);
        Mediator.PerformQuery(this, query);
        return query.Stats;
    }
        
    public bool Contains(Stat.StatType type)
    {
        return _base.Contains(type);
    }

    public IEnumerable<Stat.StatType> Types => _base.Types;
    public IEnumerable<Stat> Enumerable => _base.Enumerable;
}