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
            if (baseStats.TryGetValue(type, out var stat))
            {
                var query = new Stat.Query(stat);
                modifiers.PerformQuery(this, query);
                return query;
            }

            else throw new Exception($"Stat of type {type} not found.");
        }
    }
}