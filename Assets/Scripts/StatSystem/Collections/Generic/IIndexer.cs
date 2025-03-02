namespace StatSystem.Collections.Generic
{
    // public interface IIndexer<T> where T : IStat
    // {
    //     T this[StatType type] { get; set; }
    // }

    public interface IIndexer
    {
        Stat this[StatType type] { get; set; }
    }
}