using System;

public readonly partial struct Stat
{
    private readonly StatType _type;
    private readonly float _value;
    private readonly float? _min;
    private readonly float? _max;
    private readonly int _decimalPlaces;

    public readonly StatType Type => _type;

    public readonly float Value => _value;

    public readonly float? Min => _min;

    public readonly float? Max => _max;

    public Stat(StatType type, float value, float? min = 0, float? max = null, int decimalPlaces = 0)
    {
        if (decimalPlaces < 0)
            throw new ArgumentException("Decimal places cannot be negative.", nameof(decimalPlaces));

        _type = type;
        _decimalPlaces = decimalPlaces;

        if (max.HasValue && min.HasValue && max.Value < min.Value)
        {
            throw new ArgumentException($"max ({max.Value}) cannot be less than min ({min.Value}).", nameof(min));
        }

        _max = max;
        _min = min;
        _value = 0;
        value = Max.HasValue ? MathF.Min(Max.Value, value) : value;
        value = Min.HasValue ? MathF.Max(Min.Value, value) : value;
        _value = MathF.Round(value, _decimalPlaces);
    }

    public Stat UpdateValue(float delta)
    {
        return new Stat(Type, Value + delta, Min, Max, _decimalPlaces);
    }

    public static Stat operator +(Stat a, Stat b)
    {
        if (a.Type != b.Type)
        {
            throw new InvalidOperationException($"Cannot add Stats of different types: {a.Type} and {b.Type}");
        }

        return new Stat(a.Type, a.Value + b.Value, a.Min, a.Max, a._decimalPlaces);
    }

    public static Stat operator -(Stat a, Stat b)
    {
        if (a.Type != b.Type)
        {
            throw new InvalidOperationException($"Cannot subtract Stats of different types: {a.Type} and {b.Type}");
        }

        return new Stat(a.Type, a.Value - b.Value, a.Min, a.Max, a._decimalPlaces);
    }

    public static Stat operator +(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value + value, stat.Min, stat.Max, stat._decimalPlaces);
    }

    public static Stat operator +(float value, Stat stat)
    {
        return stat + value; // Reuse the existing logic
    }

    public static Stat operator -(Stat stat, float value)
    {
        return new Stat(stat.Type, stat.Value - value, stat.Min, stat.Max, stat._decimalPlaces);
    }

    public static Stat operator -(float value, Stat stat)
    {
        return new Stat(stat.Type, value - stat.Value, stat.Min, stat.Max, stat._decimalPlaces);
    }

    public override string ToString()
    {
        return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
    }
}

