using System.Collections.Generic;

namespace StatSystem
{
    public partial class Stats
    {
        public interface IReadOnlyStatCollection : IEnumerable<Stat.IStat>
        {
            Stat.IStat this[Stat.StatType type] { get; }
            IEnumerable<Stat.StatType> Types { get; }
            bool Contains(Stat.StatType type);
            bool TryGetStat(Stat.StatType type, out Stat.IStat stat);
        }
        public interface IReadOnlyStatCollection<T> : IEnumerable<T> where T : Stat.IStat
        {
            T this[Stat.StatType type] { get; }
            IEnumerable<Stat.StatType> Types { get; }
            bool Contains(Stat.StatType type);
            bool TryGetStat(Stat.StatType type, out T stat);
        }
    }
}