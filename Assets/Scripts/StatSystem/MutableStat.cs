namespace StatSystem
{
    /// <summary>
    /// A Stat but with mutable properties, except for Type and Precision. Can be converted back to an immutable Stat. 
    /// </summary>
    public class MutableStat : IStat
    {
        public Stat.StatType Type { get; }
        public float Value { get; set; }
        public float? Min { get; set; }
        public float? Max { get; set; }
        public int Precision { get; }

        /// <summary>
        /// Converts a Stat into a ModifiableStat class. 
        /// </summary>
        public MutableStat(IStat stat)
        {
            Type = stat.Type;
            Value = stat.Value;
            Min = stat.Min;
            Max = stat.Max;
            Precision = stat.Precision;
        }

        public MutableStat(Stat.StatType type, float value, float? min = 0, float? max = null, int precision = 0)
        {
            Type = type;
            Value = value;
            Min = min;
            Max = max;
            Precision = precision;
        }

        public static implicit operator Stat(MutableStat stat)
        {
            return new Stat(type: stat.Type, value: stat.Value, min: stat.Min, max: stat.Max, precision: stat.Precision);
        }

        public override string ToString()
        {
            return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
        }
    }
}