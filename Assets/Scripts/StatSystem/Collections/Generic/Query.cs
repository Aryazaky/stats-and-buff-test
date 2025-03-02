using System;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem.Collections.Generic
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    // public class Query<T> : IQuery<T> where T : IStat
    // {
    //     public IStatCollection<T> TemporaryStats { get; }
    //     public IIndexer<T> ReferenceStats { get; }
    //     public IEnumerable<StatType> Types { get; }
    //     public IReadOnlyWorldContexts WorldContexts { get; }
    //
    //     public Query(IStatCollection<T> stats, IReadOnlyWorldContexts worldContexts, params StatType[] types)
    //     {
    //         WorldContexts = worldContexts;
    //         Types = types;
    //         TemporaryStats = new StatCollection<T>(stats); // New to copy and start a new journey
    //         ReferenceStats = stats;
    //
    //         if (types.Any(type => !stats.Contains(type)))
    //         {
    //             throw new ArgumentOutOfRangeException(nameof(types),
    //                 $"{nameof(stats)} does not have all types requested.");
    //         }
    //     }
    // }
}