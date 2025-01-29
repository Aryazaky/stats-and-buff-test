using System.Collections;
using System.Collections.Generic;

namespace StatSystem
{
    public interface IReadOnlyStatCollection<T> : IEnumerable<T> where T : IStat
    {
        T this[Stat.StatType type] { get; }
        IEnumerable<Stat.StatType> Types { get; }
        bool Contains(Stat.StatType type);
        bool TryGetStat(Stat.StatType type, out T stat);
    }

    public interface IReadOnlyStatCollection : IEnumerable
    {
        IStat this[Stat.StatType type] { get; }
        IEnumerable<Stat.StatType> Types { get; }
        bool Contains(Stat.StatType type);
    }
}