namespace StatSystem.Collections
{
    public interface IStatCollection : IReadOnlyStatCollection
    {
        new IStat this[Stat.StatType type] { get; set; }
    }
}