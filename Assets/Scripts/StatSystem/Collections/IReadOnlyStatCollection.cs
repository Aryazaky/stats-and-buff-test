using System.Collections;
using System.Collections.Generic;

namespace StatSystem
{
    public interface IReadOnlyStatCollection : IEnumerable
    {
        IStat this[StatType type] { get; }
        IEnumerable<StatType> Types { get; }
        bool Contains(StatType type);
    }
}