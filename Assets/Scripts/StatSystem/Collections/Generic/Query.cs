using System;
using System.Linq;

namespace StatSystem.Collections.Generic
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query<T> where T : IStat
    {
        public StatCollection<T> Stats { get; }
        public Indexer<T> BaseStats { get; }
        public StatType[] Types { get; }
        public WorldContexts WorldContexts { get; }

        public Query(IStatCollection<T> stats, WorldContexts worldContexts, params StatType[] types)
        {
            WorldContexts = worldContexts;
            Types = types;
            Stats = new StatCollection<T>(stats);
            BaseStats = new Indexer<T>(stats);

            if (types.Any(type => !stats.Contains(type)))
            {
                throw new ArgumentOutOfRangeException(nameof(types),
                    $"{nameof(stats)} does not have all types requested.");
            }
        }
    }
}