using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem.Collections
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query : IQuery
    {
        public StatCollection TemporaryStats { get; }
        public StatCollection ReferenceStats { get; }
        public IEnumerable<StatType> Types { get; }
        public IReadOnlyWorldContexts WorldContexts { get; }

        public Query(StatCollection stats, IReadOnlyWorldContexts worldContexts, params StatType[] types)
        {
            WorldContexts = worldContexts;
            Types = types;
            TemporaryStats = new StatCollection(stats.ToStruct());
            ReferenceStats = stats;

            if (types.Any(type => !stats.Contains(type)))
            {
                throw new ArgumentOutOfRangeException(nameof(types),
                    $"{nameof(stats)} does not have all types requested.");
            }
        }
    }
}