using System;

public readonly partial struct Stat
{
    public class ModifiableStat : IStat
    {
        public StatType Type { get; }
        public float Value { get; set; }
        public float? Min { get; set; }
        public float? Max { get; set; }
        public float Precision { get; }

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