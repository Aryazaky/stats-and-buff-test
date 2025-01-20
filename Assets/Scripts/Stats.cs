using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stats
{
    private class StatPair
    {
        public Stat Base;
        public Stat Processed;

        public StatPair(Stat baseStat, Stat? cache = null)
        {
            Base = baseStat;
            Processed = cache ?? baseStat;
        }
    }

    public class Dict
    {
        private readonly Dictionary<Stat.StatType, Stat> _stats;

        public Dict(params Stat[] stats)
        {
            _stats = stats.ToDictionary(stat => stat.Type);
        }

        public Dict(IEnumerable<Stat> stats)
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

    private readonly Dict _stats;
    private Dict _modified;

    public Stat.Mediator Mediator { get; } = new();

    public Stats(params Stat[] stats)
    {
        _stats = new Dict(stats);
        _modified = new Dict(stats);
        // var zeroes = stats.Select(stat => stat.SetValue(0));
        // _modified = new Dict(zeroes);
    }
    
    public Stats(IEnumerable<Stat> stats)
    {
        _stats = new Dict(stats);
        _modified = new Dict(stats);
        // var zeroes = stats.Select(stat => stat.SetValue(0));
        // _modified = new Dict(zeroes);
    }

    public Stat this[Stat.StatType type]
    {
        get => _stats[type];
        set => _stats[type] = value;
    }

    public void Update(params Stat.StatType[] types)
    {
        if (!types.Any()) types = Types.ToArray();
        foreach (var type in types)
        {
            _modified[type] = PerformQuery(_stats[type]);
        }
    }

    private Stat PerformQuery(Stat stat)
    {
        var query = new Stat.Query(this, stat);
        Mediator.PerformQuery(this, query);
        return query.Stat;
    }
        
    public bool Contains(Stat.StatType type)
    {
        return _stats.Contains(type);
    }

    public IEnumerable<Stat.StatType> Types => _stats.Types;
    public IEnumerable<Stat> Enumerable => _stats.Enumerable;
}