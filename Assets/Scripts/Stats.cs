using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stats : IEnumerable<Stat>
{
    public struct StatDictionary<T> : IReadOnlyDictionary<Stat.StatType, T> where T : Stat.IStat
    {
        private readonly Dictionary<Stat.StatType, T> _stats;

        public StatDictionary(params T[] stats) => _stats = stats.ToDictionary(stat => stat.Type);

        public StatDictionary(IEnumerable<T> stats) => _stats = stats.ToDictionary(stat => stat.Type);

        public T this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
        
        public bool ContainsKey(Stat.StatType type) => _stats.ContainsKey(type);
        public bool TryGetValue(Stat.StatType type, out T stat) => _stats.TryGetValue(type, out stat);
        public IEnumerable<Stat.StatType> Keys => _stats.Keys;
        public IEnumerable<T> Values => _stats.Values;
        public IEnumerator<KeyValuePair<Stat.StatType, T>> GetEnumerator() => _stats.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => _stats.Count;
    }

    public class ModifiableStatDictionary : IReadOnlyDictionary<Stat.StatType, Stat.ModifiableStat>
    {
        private StatDictionary<Stat.ModifiableStat> _statDictionary;

        public ModifiableStatDictionary(StatDictionary<Stat> statDictionary)
        {
            _statDictionary = new StatDictionary<Stat.ModifiableStat>(statDictionary.Select(stat => new Stat.ModifiableStat(stat.Value)));
        }

        public ModifiableStatDictionary(Stats stats)
        {
            _statDictionary = new StatDictionary<Stat.ModifiableStat>(stats.Select(stat => new Stat.ModifiableStat(stat)));
        }
        
        public Stat.ModifiableStat this[Stat.StatType type]
        {
            get => _statDictionary[type];
            set => _statDictionary[type] = value;
        }

        public IEnumerable<Stat.StatType> Keys => _statDictionary.Keys;

        public IEnumerable<Stat.ModifiableStat> Values => _statDictionary.Values;

        public bool ContainsKey(Stat.StatType type) => _statDictionary.ContainsKey(type);

        public bool TryGetValue(Stat.StatType type, out Stat.ModifiableStat stat) => _statDictionary.TryGetValue(type, out stat);

        public static implicit operator StatDictionary<Stat>(ModifiableStatDictionary wrapper)
        {
            return new StatDictionary<Stat>(wrapper._statDictionary.Select(keyValuePair => (Stat)keyValuePair.Value));
        }

        public IEnumerator<KeyValuePair<Stat.StatType, Stat.ModifiableStat>> GetEnumerator()
        {
            return _statDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_statDictionary).GetEnumerator();

        public int Count => _statDictionary.Count;
    }

    public class StatsIndexer
    {
        private readonly Stats _stats;
        public StatsIndexer(Stats stats)
        {
            _stats = stats;
        }
        public Stat this[Stat.StatType type]
        {
            get => _stats._base[type];
            set => _stats._base[type] = value;
        }
    }

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
        return query.ModifiableStats[type];
    }

    private StatDictionary<Stat> PerformQuery()
    {
        var query = new Stat.Query(this);
        Mediator.PerformQuery(new NoQueryArgs(this, query));
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