namespace StatSystem.Collections
{
    public interface IStatIndexer
    {
        Stat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out Stat stat);
    }
    public interface IMutableStatIndexer
    {
        MutableStat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out MutableStat stat);
    }
}