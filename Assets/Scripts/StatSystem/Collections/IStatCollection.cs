namespace StatSystem.Collections
{
    public interface IStatCollection : IBaseStatCollection
    {
        Stat this[StatType type] { get; set; }
        bool TryGetStat(StatType type, out Stat stat);
    }
}