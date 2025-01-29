using System;
using System.Linq;

namespace StatSystem.Collections
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query
    {
        public StatCollection Stats { get; }
        public Indexer BaseStats { get; }
        public StatType[] Types { get; }
        public WorldContexts WorldContexts { get; }

        public Query(IStatCollection stats, WorldContexts worldContexts, params StatType[] types)
        {
            WorldContexts = worldContexts;
            Types = types;
            Stats = new StatCollection(stats);
            BaseStats = new Indexer(stats);

            if (types.Any(type => !stats.Contains(type)))
            {
                throw new ArgumentOutOfRangeException(nameof(types),
                    $"{nameof(stats)} does not have all types requested.");
            }
        }
    }
}