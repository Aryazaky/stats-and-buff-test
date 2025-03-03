using System.Collections.Generic;

namespace StatSystem.Collections
{
    public interface IQuery
    {
        public StatCollection DisplayedStats { get; }
        public StatCollection BaseStats { get; }
        IEnumerable<StatType> Types { get; }
        IReadOnlyWorldContexts WorldContexts { get; }
    }
}