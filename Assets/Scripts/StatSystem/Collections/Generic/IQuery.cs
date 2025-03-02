using System.Collections.Generic;

namespace StatSystem.Collections.Generic
{
    public interface IQuery<T> where T : IStat
    {
        public IStatCollection<T> TemporaryStats { get; }
        public IIndexer<T> ReferenceStats { get; }
        IEnumerable<StatType> Types { get; }
        IReadOnlyWorldContexts WorldContexts { get; }
    }
}