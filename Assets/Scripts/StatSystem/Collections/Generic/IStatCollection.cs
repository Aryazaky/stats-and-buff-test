namespace StatSystem.Collections.Generic
{
    public interface IStatCollection<T> : IReadOnlyStatCollection<T> where T : IStat
    {
        new T this[StatType type] { get; set; }
    }
}