using System.Collections.Generic;

namespace StatSystem.Collections
{
    public interface IQuery
    {
        public StatCollection TemporaryStats { get; }
        public StatCollection ReferenceStats { get; }
        IEnumerable<StatType> Types { get; }
        IReadOnlyWorldContexts WorldContexts { get; }
    }
}