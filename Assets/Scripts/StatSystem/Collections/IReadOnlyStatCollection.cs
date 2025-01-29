using System.Collections;
using System.Collections.Generic;

namespace StatSystem
{
    public interface IReadOnlyStatCollection : IEnumerable
    {
        IStat this[Stat.StatType type] { get; }
        IEnumerable<Stat.StatType> Types { get; }
        bool Contains(Stat.StatType type);
    }
}