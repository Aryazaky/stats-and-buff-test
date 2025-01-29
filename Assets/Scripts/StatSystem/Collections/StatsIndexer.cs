namespace StatSystem.Collections
{
    public class StatsIndexer
    {
        private readonly IStatCollection _stats;
        public StatsIndexer(IStatCollection stats)
        {
            _stats = stats;
        }
        public IStat this[Stat.StatType type]
        {
            get => _stats[type];
            set => _stats[type] = value;
        }
    }
}