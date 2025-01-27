namespace StatSystem
{
    public partial class Stats
    {
        public class StatsIndexer
        {
            private readonly IStatCollection _stats;
            public StatsIndexer(IStatCollection stats)
            {
                _stats = stats;
            }
            public Stat.IStat this[Stat.StatType type]
            {
                get => _stats[type];
                set => _stats[type] = value;
            }
        }
    }
}