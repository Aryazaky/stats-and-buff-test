using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem.Collections
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query : IQuery
    {
        public StatCollection DisplayedStats { get; }
        public StatCollection BaseStats { get; }
        public IEnumerable<StatType> Types { get; }
        public IReadOnlyWorldContexts WorldContexts { get; }

        public Query(StatCollection stats, IReadOnlyWorldContexts worldContexts, params StatType[] types)
        {
            WorldContexts = worldContexts;
            Types = types;
            DisplayedStats = new StatCollection(stats.ToStruct());
            BaseStats = stats;

            if (types.Any(type => !stats.Contains(type)))
            {
                throw new ArgumentOutOfRangeException(nameof(types),
                    $"{nameof(stats)} does not have all types requested.");
            }
        }
    }
}