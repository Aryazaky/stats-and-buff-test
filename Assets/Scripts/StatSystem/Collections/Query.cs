namespace StatSystem.Collections
{
    /// A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    internal class Query : IQuery
    {
        public StatCollection QueriedStats { get; }
        public StatCollection BaseStats { get; }
        public IReadOnlyWorldContexts WorldContexts { get; }

        public Query(StatCollection stats, IReadOnlyWorldContexts worldContexts)
        {
            WorldContexts = worldContexts;
            QueriedStats = new StatCollection(stats.ToStruct());
            BaseStats = stats;
        }
    }
}