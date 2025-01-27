namespace StatSystem
{
    public readonly partial struct Stat
    {
        /// <summary>
        /// A Stat but with mutable properties, except for Type and Precision. Can be converted back to an immutable Stat. 
        /// </summary>
        public class ModifiableStat : IStat
        {
            public StatType Type { get; }
            public float Value { get; set; }
            public float? Min { get; set; }
            public float? Max { get; set; }
            public float Precision { get; }

            /// <summary>
            /// Converts a Stat into a ModifiableStat class. 
            /// </summary>
            public ModifiableStat(Stat stat)
            {
                Value = stat.Value;
                Precision = stat._precision;
                Type = stat.Type;
                Min = stat.Min;
                Max = stat.Max;
            }

            public static implicit operator Stat(ModifiableStat stat)
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