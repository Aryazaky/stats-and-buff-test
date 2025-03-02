using System.Collections;
using System.Collections.Generic;
using StatSystem.Collections.Generic;

namespace StatSystem.Collections
{
    public interface IReadOnlyStatCollection
    {
        IEnumerable<StatType> Types { get; }
        bool Contains(params StatType[] type);
        bool TryGetStat(StatType type, out Stat stat);
    }
}