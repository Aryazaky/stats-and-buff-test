using System.Collections.Generic;
using StatSystem.Collections.Generic;

namespace StatSystem.Collections
{
    public interface IQuery
    {
        public IStatCollection Stats { get; }
        public IIndexer BaseStats { get; }
        IEnumerable<StatType> Types { get; }
        IReadOnlyWorldContexts WorldContexts { get; }
    }
}