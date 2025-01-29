using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class StatQuery
    {
        public StatCollection Stats { get; }
        public StatsIndexer BaseStats { get; }
        public Stat.StatType[] Types { get; }
        public WorldContexts WorldContexts { get; }

        public StatQuery(IStatCollection stats, WorldContexts worldContexts, params Stat.StatType[] types)
        {
            WorldContexts = worldContexts;
            Types = types;
            Stats = new StatCollection(stats);
            BaseStats = new StatsIndexer(stats);

            if (types.Any(type => !stats.Contains(type)))
            {
                throw new ArgumentOutOfRangeException(nameof(types),
                    $"{nameof(stats)} does not have all types requested.");
            }
        }
    }
}