public readonly partial struct Stat
{
    // A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query
    {
        // Get only properties
        public CapturedStats Stats { get; }
        public MutableStat Stat { get; }
        public Query(Stat stat, Stats stats)
        {
            this.Stat = new MutableStat(stat);
            this.Stats = new CapturedStats(stats);
        }
    }
}
