using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public readonly partial struct Stat
    {
        // A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
        public class Query
        {
            public Stats.StatCollection Stats { get; }
            public Stats.StatsIndexer BaseStats { get; }
            public StatType[] Types { get; }
            public WorldContexts WorldContexts { get; }

            public Query(Stats.IStatCollection stats, WorldContexts worldContexts, params StatType[] types)
            {
                WorldContexts = worldContexts;
                Types = types;
                Stats = new Stats.StatCollection(stats);
                BaseStats = new Stats.StatsIndexer(stats);

                if (types.Any(type => !stats.Contains(type)))
                {
                    throw new ArgumentOutOfRangeException(nameof(types),
                        $"{nameof(stats)} does not have all types requested.");
                }
            }
        }
    }
}