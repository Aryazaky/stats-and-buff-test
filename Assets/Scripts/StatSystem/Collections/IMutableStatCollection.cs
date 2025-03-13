namespace StatSystem.Collections
{
    public interface IMutableStatCollection : IReadOnlyStatCollection
    {
        MutableStat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out MutableStat stat);
    }
}