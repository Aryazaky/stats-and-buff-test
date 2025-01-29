using System.Collections.Generic;

namespace StatSystem.Collections.Generic
{
    public interface IReadOnlyStatCollection<T> : IEnumerable<T> where T : IStat
    {
        T this[StatType type] { get; }
        IEnumerable<StatType> Types { get; }
        bool Contains(StatType type);
        bool TryGetStat(StatType type, out T stat);
    }
}