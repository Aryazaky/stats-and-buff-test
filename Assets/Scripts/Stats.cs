using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stats
{
    private readonly Dictionary<Stat.StatType, Stat> baseStats;
    private readonly Stat.Mediator mediator = new();
    private readonly HashSet<Stat.StatType> currentlyAccessed = new();

    public Stat.Mediator Mediator => mediator;

    public Stats(params Stat[] stats)
    {
        baseStats = stats.ToDictionary(stat => stat.Type);
    }

    public Stat this[Stat.StatType type]
    {
        get
        {
            if (currentlyAccessed.Contains(type))
            {
                throw new InvalidOperationException($"Circular access detected for stat: {type}. Check modifiers and prerequisites.");
            }

            if (TryGetBaseStat(type, out var stat))
            {
                currentlyAccessed.Add(type);
                try
                {
                    var query = new Stat.Query(stat, this);
                    mediator.PerformQuery(this, query);
                    return query.Stat;
                }
                finally
                {
                    currentlyAccessed.Remove(type);
                }
            }

            throw new Exception($"Stat of type {type} not found.");
        }
        set 
        { 
            if (Contains(type))
            {
                // TODO: Make some processing? Or a warning because a set accessor is destructive?
                if (value.Type == type)
                {
                    baseStats[type] = value;
                }
                else throw new ArgumentException($"Type mismatch. Tried to assign a {type} with a {value}.");
            }
            else throw new Exception($"Unable to assign stat of type {type}. It was not exist before.");
        }
    }

    public void ApplyChange(Stat.StatType type, float delta)
    {
        if (TryGetBaseStat(type, out var stat))
        {
            baseStats[type] = stat.UpdateValue(delta);
        }
        else throw new Exception($"Stat of type {type} not found.");
    }

    public bool TryGetBaseStat(Stat.StatType type, out Stat stat)
    {
        return baseStats.TryGetValue(type, out stat);
    }

    public bool Contains(Stat.StatType type)
    {
        return baseStats.ContainsKey(type);
    }

    public IEnumerable<Stat.StatType> Types => baseStats.Keys;
}

public class CapturedStats
{
    private readonly Dictionary<Stat.StatType, Stat> stats;

    public CapturedStats(Stats stats)
    {
        if (stats == null) throw new ArgumentNullException(nameof(stats));

        this.stats = new Dictionary<Stat.StatType, Stat>();
        foreach (var type in stats.Types)
        {
            this.stats[type] = stats[type];
        }
    }

    public Stat this[Stat.StatType type]
    {
        get
        {
            if (stats.TryGetValue(type, out var value))
            {
                return value;
            }
            else
            {
                throw new Exception($"Stat of type {type} not found.");
            }
        }
        set
        {
            if (stats.ContainsKey(type))
            {
                if (value.Type == type)
                {
                    stats[type] = value;
                }
                else throw new ArgumentException($"Type mismatch. Tried to assign a {type} with a {value}.");
            }
        }
    }

    public IEnumerable<KeyValuePair<Stat.StatType, Stat>> Values() => stats;
}