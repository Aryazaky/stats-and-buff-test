namespace StatSystem
{
    public partial class Stats
    {
        public class StatsIndexer
        {
            private readonly Stats _stats;
            public StatsIndexer(Stats stats)
            {
                _stats = stats;
            }
            public Stat this[Stat.StatType type]
            {
                get => _stats._base[type];
                set => _stats._base[type] = value;
            }
        }
    }
}