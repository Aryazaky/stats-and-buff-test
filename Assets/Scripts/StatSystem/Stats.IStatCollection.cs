namespace StatSystem
{
    public partial class Stats
    {
        public interface IStatCollection : IReadOnlyStatCollection
        {
            new Stat.IStat this[Stat.StatType type] { get; set; }
        }
        public interface IStatCollection<T> : IReadOnlyStatCollection<T> where T : Stat.IStat
        {
            new T this[Stat.StatType type] { get; set; }
        }
    }
}