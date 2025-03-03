namespace StatSystem.Collections
{
    public interface IIndexer
    {
        Stat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out Stat stat);
    }
}