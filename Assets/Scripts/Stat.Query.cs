public readonly partial struct Stat
{
    // A class with properties that can be viewed or freely be changed while it undergone a journey through multiple modifiers. 
    public class Query
    {
        public StatCollection.ModifiableStats Stats { get; }
        public StatCollection.BaseStatsIndexer StatsRef { get; }
        public StatType[] Types { get; }

        public Query(StatCollection statCollection, params StatType[] types)
        {
            Types = types;
            Stats = new StatCollection.ModifiableStats(statCollection);
            StatsRef = new StatCollection.BaseStatsIndexer(statCollection);
            Types = types;
        }
    }
}