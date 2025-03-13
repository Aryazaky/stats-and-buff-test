namespace StatSystem.Collections
{
    public interface IWriteOnlyStatIndexer<in T>
    {
        T this[StatType type] { set; }
    }
}