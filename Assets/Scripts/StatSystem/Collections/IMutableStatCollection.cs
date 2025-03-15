namespace StatSystem.Collections
{
    public interface IMutableStatCollection : IBaseStatCollection
    {
        MutableStat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out MutableStat stat);
    }
}