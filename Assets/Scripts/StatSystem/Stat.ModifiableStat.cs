namespace StatSystem
{
    public readonly partial struct Stat
    {
        /// <summary>
        /// A Stat but with mutable properties, except for Type and Precision. Can be converted back to an immutable Stat. 
        /// </summary>
        public class MutableStat : IStat
        {
            public StatType Type { get; }
            public float Value { get; set; }
            public float? Min { get; set; }
            public float? Max { get; set; }
            public int Precision { get; }

            /// <summary>
            /// Converts a Stat into a ModifiableStat class. 
            /// </summary>
            public MutableStat(IStat stat)
            {
                Value = stat.Value;
                Precision = stat.Precision;
                Type = stat.Type;
                Min = stat.Min;
                Max = stat.Max;
            }

            public static implicit operator Stat(MutableStat stat)
            {
                return new Stat(type: stat.Type, value: stat.Value, min: stat.Min, max: stat.Max);
            }

            public override string ToString()
            {
                return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
            }
        }
    }
}