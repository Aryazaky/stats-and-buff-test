using System;
using System.Linq;

namespace StatSystem
{
    public readonly partial struct Stat
    {
        // A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
        public class Query
        {
            public Stats.ModifiableStatDictionary ModifiableStats { get; }
            public Stats.StatsIndexer BaseStats { get; }
            public StatType[] Types { get; }

            public Query(Stats stats, params StatType[] types)
            {
                Types = types;
                ModifiableStats = new Stats.ModifiableStatDictionary(stats);
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