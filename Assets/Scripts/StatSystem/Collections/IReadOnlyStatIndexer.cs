namespace StatSystem.Collections
{
    public interface IReadOnlyStatIndexer<T>
    {
        T this[StatType type] { get; }
        bool TryGetStat(StatType type, out T stat);
    }
}