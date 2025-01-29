namespace StatSystem.Collections
{
    public class Indexer
    {
        private readonly IStatCollection _stats;
        public Indexer(IStatCollection stats)
        {
            _stats = stats;
        }
        public IStat this[StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
    }
}