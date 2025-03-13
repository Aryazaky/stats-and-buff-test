namespace StatSystem.Collections
{
    public interface IStatCollection : IReadOnlyStatCollection
    {
        Stat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out Stat stat);
    }
}