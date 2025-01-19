using System;

public readonly partial struct Stat
{
    public class MutableStat
    {
        public StatType Type { get; }
        public float Value;
        public float? Min;
        public float? Max;
        public float precision;

        public MutableStat(Stat stat)
        {
            this.Value = stat.Value;
            this.precision = stat.precision;
            this.Type = stat.type;
            this.Min = stat.Min;
            this.Max = stat.Max;
        }

        public static implicit operator Stat(MutableStat stat)
        {
            return new Stat(type: stat.Type, value: stat.Value, min: stat.Min, max: stat.Max);
        }
    }

    private readonly StatType type;
    private readonly float value;
    private readonly float? min;
    private readonly float? max;
    private readonly int precision;

    public readonly StatType Type => type;

    public readonly float Value => value;

    public readonly float? Min => min;

    public readonly float? Max => max;

    public Stat(StatType type, float value, float? min = 0, float? max = null, int precision = 0)
    {
        if (precision < 0)
            throw new ArgumentException("Decimal places cannot be negative.", nameof(precision));

        this.type = type;
        this.precision = precision;

        if (max.HasValue && min.HasValue && max.Value < min.Value)
        {
            throw new ArgumentException($"max ({max.Value}) cannot be less than min ({min.Value}).", nameof(min));
        }

        this.max = max;
        this.min = min;
        this.value = 0;
        value = Max.HasValue ? MathF.Min(Max.Value, value) : value;
        value = Min.HasValue ? MathF.Max(Min.Value, value) : value;
        this.value = MathF.Round(value, this.precision);
    }

    public Stat UpdateValue(float delta)
    {
        return new Stat(Type, Value + delta, Min, Max, precision);
    }

    public static Stat operator +(Stat a, Stat b)
    {
        if (a.Type != b.Type)
        {
            throw new InvalidOperationException($"Cannot add Stats of different types: {a.Type} and {b.Type}");
        }

        return new Stat(a.Type, a.Value + b.Value, a.Min, a.Max, a.precision);
    }

    public static Stat operator -(Stat a, Stat b)
    {
        if (a.Type != b.Type)
        {
            throw new InvalidOperationException($"Cannot subtract Stats of different types: {a.Type} and {b.Type}");
        }

        return new Stat(a.Type, a.Value - b.Value, a.Min, a.Max, a.precision);
    }

    public static Stat operator +(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value + value, stat.Min, stat.Max, stat.precision);
    }

    public static Stat operator +(float value, Stat stat)
    {
        return stat + value;
    }

    public static Stat operator -(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value - value, stat.Min, stat.Max, stat.precision);
    }

    public static Stat operator -(float value, Stat stat)
    {
        return new Stat(stat.Type, value - stat.Value, stat.Min, stat.Max, stat.precision);
    }

    public static Stat operator *(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value * value, stat.Min, stat.Max, stat.precision);
    }

    public static Stat operator *(float value, Stat stat)
    {
        return stat * value;
    }

    public static Stat operator /(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value / value, stat.Min, stat.Max, stat.precision);
    }

    public static Stat operator /(float value, Stat stat)
    {
        return new Stat(stat.Type, value / stat.Value, stat.Min, stat.Max, stat.precision);
    }

    public override string ToString()
    {
        return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
    }
}

