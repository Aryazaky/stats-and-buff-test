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
                    var query = new Stat.Query(stat);
                    mediator.PerformQuery(this, query);
                    return query;
                }
                finally
                {
                    currentlyAccessed.Remove(type);
                }
            }

            throw new Exception($"Stat of type {type} not found.");
        }
    }

    public bool TryGetBaseStat(Stat.StatType type, out Stat stat)
    {
        return baseStats.TryGetValue(type, out stat);
    }

    public bool Contains(Stat.StatType type)
    {
        return baseStats.ContainsKey(type);
    }
}