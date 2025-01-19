public partial struct Stat
{
    public class Query
    {
        public StatType type { get; }
        public float value;
        public float? min;
        public float? max;
        public Query(StatType type, float value, float? min = null, float? max = null)
        {
            this.type = type;
            this.value = value;
            this.min = min;
            this.max = max;
        }
        public Query(Stat stat)
        {
            this.type = stat.Type;
            this.value = stat.Value;
            this.min = stat.Min;
            this.max = stat.Max;
        }
        public static implicit operator Stat(Query query)
        {
            return new Stat(type: query.type, value: query.value, min: query.min, max: query.max);
        }
    }
}
