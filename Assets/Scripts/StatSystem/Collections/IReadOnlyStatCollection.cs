using System.Collections.Generic;

namespace StatSystem.Collections
{
    public interface IReadOnlyStatCollection
    {
        IEnumerable<StatType> Types { get; }
        bool Contains(params StatType[] type);
    }
}