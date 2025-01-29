namespace StatSystem.Collections.Generic
{
    public class Indexer<T> where T : IStat
    {
        private readonly IStatCollection<T> _stats;
        public Indexer(IStatCollection<T> stats)
        {
            _stats = stats;
        }
        public T this[StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
    }
}