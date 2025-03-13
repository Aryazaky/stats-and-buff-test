namespace StatSystem.Collections
{
    public interface IQuery
    {
        public StatCollection QueriedStats { get; }
        public StatCollection BaseStats { get; }
        IReadOnlyWorldContexts WorldContexts { get; }
    }
}