public readonly partial struct Stat
{
    // A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query
    {
        public Stats.DataWrapper Stats { get; }
        public Stats.IndexerOnly StatsRef { get; }
        public StatType[] Types { get; }

        public Query(Stats stats, params StatType[] types)
        {
            Types = types;
            Stats = new Stats.DataWrapper(stats);
            StatsRef = new Stats.IndexerOnly(stats);
            Types = types;
        }
    }
}
