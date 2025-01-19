using System;

public partial struct Stat
{
    private StatType _type;
    private float _value;
    private float? _min;
    private float? _max;
    private int _decimalPlaces;
    public StatType Type => _type;

    public float Value
    {
        get => _value;
        set
        {
            float newValue = Max.HasValue ? MathF.Min(Max.Value, value) : value;
            newValue = Min.HasValue ? MathF.Max(Min.Value, newValue) : newValue;
            _value = MathF.Round(newValue, _decimalPlaces);
        }
    }

    public float? Min => _min;

    public float? Max => _max;

    public Stat(StatType type, float value, float? min = 0, float? max = null, int decimalPlaces = 0)
    {
        if (decimalPlaces < 0)
            throw new ArgumentException("Decimal places cannot be negative.", nameof(decimalPlaces));

        _type = type;
        _decimalPlaces = decimalPlaces;

        if (max.HasValue && min.HasValue && max.Value < min.Value)
        {
            throw new ArgumentException($"max ({max.Value}) cannot be less than min ({min.Value})");
        }

        _max = max;
        _min = min;
        _value = 0;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}";
    }
}

