using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Stats
{
    private readonly Dictionary<Stat.StatType, Stat> baseStats;
    private readonly Stat.Modifiers modifiers = new();

    public Stat.Modifiers Modifiers => modifiers;

    public Stats(params Stat[] stats)
    {
        baseStats = stats.ToDictionary(stat => stat.Type);
    }

    public Stat this[Stat.StatType type]
    {
        get
        {
            if (TryGetBaseStat(type, out var stat))
            {
                var query = new Stat.Query(stat); // Error Stackoverflow
                modifiers.PerformQuery(this, query);
                return query;
            }

            else throw new Exception($"Stat of type {type} not found.");
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