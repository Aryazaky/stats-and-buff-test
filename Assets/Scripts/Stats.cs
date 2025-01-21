using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stats
{
    public struct Data
    {
        private readonly Dictionary<Stat.StatType, Stat> _stats;

        public Data(params Stat[] stats)
        {
            _stats = stats.ToDictionary(stat => stat.Type);
        }

        public Data(IEnumerable<Stat> stats)
        {
            _stats = stats.ToDictionary(stat => stat.Type);
        }
    
        public Stat this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
        
        public bool Contains(Stat.StatType type)
        {
            return _stats.ContainsKey(type);
        }

        public IEnumerable<Stat.StatType> Types => _stats.Keys;
        public IEnumerable<Stat> Enumerable => _stats.Values;
    }

    public class DataWrapper
    {
        private Data _data;

        public DataWrapper(Data data)
        {
            _data = data;
        }

        public DataWrapper(Stats stats)
        {
            _data = stats._base;
        }
        
        public Stat this[Stat.StatType type]
        {
            get => _data[type];
            set => _data[type] = value;
        }
        
        public static implicit operator Data(DataWrapper data)
        {
            return data._data;
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

    private Data _base;
    private Data _modified;

    public Stat.Mediator Mediator { get; } = new();

    public Stats(params Stat[] stats)
    {
        _base = new Data(stats);
        _modified = new Data(stats);
    }
    
    public Stats(IEnumerable<Stat> stats)
    {
        _base = new Data(stats);
        _modified = new Data(stats);
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

    private Data PerformQuery()
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