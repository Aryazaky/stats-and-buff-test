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

    public override string ToString()
    {
        return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
    }
}

