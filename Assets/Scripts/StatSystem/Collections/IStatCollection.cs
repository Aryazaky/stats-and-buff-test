namespace StatSystem.Collections
{
    public interface IStatCollection : IReadOnlyStatCollection
    {
        new IStat this[StatType type] { get; set; }
    }
}