using System;

public interface IStat
{
    public Stat.StatType Type { get; }

    public float Value { get; }
}

public readonly partial struct Stat : IStat
{
    public class MutableStat : IStat
    {
        public StatType Type { get; }
        public float Value { get; set; }
        public float? Min { get; set; }
        public float? Max { get; set; }
        public float Precision { get; }

        public MutableStat(Stat stat)
        {
            Value = stat.Value;
            Precision = stat._precision;
            Type = stat.Type;
            Min = stat.Min;
            Max = stat.Max;
        }

        public static implicit operator Stat(MutableStat stat)
        {
            return new Stat(type: stat.Type, value: stat.Value, min: stat.Min, max: stat.Max);
        }
    }

    private readonly int _precision;

    public StatType Type { get; }

    public float Value { get; }

    public float? Min { get; }

    public float? Max { get; }

    public Stat(StatType type, float value, float? min = 0, float? max = null, int precision = 0)
    {
        if (precision < 0)
            throw new ArgumentException("Decimal places cannot be negative.", nameof(precision));

        Type = type;
        _precision = precision;

        if (max.HasValue && min.HasValue && max.Value < min.Value)
        {
            throw new ArgumentException($"max ({max.Value}) cannot be less than min ({min.Value}).", nameof(min));
        }

        Max = max;
        Min = min;
        Value = 0;
        value = Max.HasValue ? MathF.Min(Max.Value, value) : value;
        value = Min.HasValue ? MathF.Max(Min.Value, value) : value;
        Value = MathF.Round(value, _precision);
    }

    public Stat UpdateValue(float delta)
    {
        return new Stat(Type, Value + delta, Min, Max, _precision);
    }

    public Stat SetValue(float value)
    {
        return new Stat(Type, value, Min, Max, _precision);
    }

    public static Stat operator +(Stat a, Stat b)
    {
        if (a.Type != b.Type)
        {
            throw new InvalidOperationException($"Cannot add Stats of different types: {a.Type} and {b.Type}");
        }

        return new Stat(a.Type, a.Value + b.Value, a.Min, a.Max, a._precision);
    }

    public static Stat operator -(Stat a, Stat b)
    {
        if (a.Type != b.Type)
        {
            throw new InvalidOperationException($"Cannot subtract Stats of different types: {a.Type} and {b.Type}");
        }

        return new Stat(a.Type, a.Value - b.Value, a.Min, a.Max, a._precision);
    }

    public static Stat operator +(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value + value, stat.Min, stat.Max, stat._precision);
    }

    public static Stat operator +(float value, Stat stat)
    {
        return stat + value;
    }

    public static Stat operator -(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value - value, stat.Min, stat.Max, stat._precision);
    }

    public static Stat operator -(float value, Stat stat)
    {
        return new Stat(stat.Type, value - stat.Value, stat.Min, stat.Max, stat._precision);
    }

    public static Stat operator *(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value * value, stat.Min, stat.Max, stat._precision);
    }

    public static Stat operator *(float value, Stat stat)
    {
        return stat * value;
    }

    public static Stat operator /(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value / value, stat.Min, stat.Max, stat._precision);
    }

    public static Stat operator /(float value, Stat stat)
    {
        return new Stat(stat.Type, value / stat.Value, stat.Min, stat.Max, stat._precision);
    }

    public override string ToString()
    {
        return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
    }
}

