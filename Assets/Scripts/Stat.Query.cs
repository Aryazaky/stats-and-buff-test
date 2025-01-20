public readonly partial struct Stat
{
    // A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query
    {
        // Get only properties
        public Stats Stats { get; }
        public Stats Modifiable { get; }
        public MutableStat Stat { get; }
        public Query(Stats stats, Stat stat)
        {
            Stat = new MutableStat(stat);
            Stats = stats;
            Modifiable = new Stats(stats.Enumerable);
        }
    }
}
