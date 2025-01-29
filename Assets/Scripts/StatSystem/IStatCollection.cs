namespace StatSystem
{
    public interface IStatCollection<T> : IReadOnlyStatCollection<T> where T : IStat
    {
        new T this[Stat.StatType type] { get; set; }
    }

    public interface IStatCollection : IReadOnlyStatCollection
    {
        new IStat this[Stat.StatType type] { get; set; }
    }
}