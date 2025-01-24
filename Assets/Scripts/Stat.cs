using System;

/// <summary>
/// An immutable stat with a type, value, and optional min/max range, supporting precision handling.
/// </summary>
public readonly partial struct Stat : Stat.IStat
{
    private readonly int _precision;

    public StatType Type { get; }

    public float Value { get; }

    public float? Min { get; }

    public float? Max { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="Stat"/>.
    /// The constructor ensures that the provided value is within the specified range (min to max, if not null),
    /// and rounds the value to the given precision. 
    /// </summary>
    /// <remarks>
    /// If the precision is negative, an exception is thrown.
    /// If the max value is less than the min value, an exception is thrown.
    /// </remarks>
    /// <param name="type">The type of the stat (e.g., health, stamina, etc.).</param>
    /// <param name="value">The value of the stat, which will be clamped between the min and max values.</param>
    /// <param name="min">The minimum allowable value for the stat (optional, defaults to 0).</param>
    /// <param name="max">The maximum allowable value for the stat (optional, defaults to null, which means no limit).</param>
    /// <param name="precision">The number of decimal places to round the value to (optional, defaults to 0).</param>
    /// <exception cref="ArgumentException">Thrown if precision is negative, or if max is less than min.</exception>
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
    
    /// <param name="delta">The value to add to existing value</param>
    /// <returns>A new Stat instance with the value adjusted by a given delta.</returns>
    public Stat UpdateValue(float delta)
    {
        return new Stat(Type, Value + delta, Min, Max, _precision);
    }

    /// <param name="value">The value to replace the existing value</param>
    /// <returns>A new Stat instance with the replaced value.</returns>
    public Stat SetValue(float value)
    {
        return new Stat(Type, value, Min, Max, _precision);
    }

    private Stat PerformOperation(Stat other, Func<float, float, float> operation)
    {
        if (Type != other.Type)
            throw new InvalidOperationException($"Cannot operate on Stats of different types: {Type} and {other.Type}");

        return new Stat(Type, operation(Value, other.Value), Min, Max, _precision);
    }

    private Stat PerformOperation(float value, Func<float, float, float> operation)
    {
        return new Stat(Type, operation(Value, value), Min, Max, _precision);
    }

    public static Stat operator +(Stat a, Stat b) => a.PerformOperation(b, (x, y) => x + y);
    public static Stat operator -(Stat a, Stat b) => a.PerformOperation(b, (x, y) => x - y);
    public static Stat operator +(Stat stat, float value) => stat.PerformOperation(value, (x, y) => x + y);
    public static Stat operator +(float value, Stat stat) => stat + value;
    public static Stat operator -(Stat stat, float value) => stat.PerformOperation(value, (x, y) => x - y);
    public static Stat operator -(float value, Stat stat) => new Stat(stat.Type, value - stat.Value, stat.Min, stat.Max, stat._precision);
    public static Stat operator *(Stat stat, float value) => stat.PerformOperation(value, (x, y) => x * y);
    public static Stat operator *(float value, Stat stat) => stat * value;
    public static Stat operator /(Stat stat, float value) => stat.PerformOperation(value, (x, y) => x / y);
    public static Stat operator /(float value, Stat stat) => new Stat(stat.Type, value / stat.Value, stat.Min, stat.Max, stat._precision);

    private bool Compare(Stat other, Func<float, float, bool> comparison)
    {
        if (Type != other.Type)
            throw new InvalidOperationException($"Cannot compare Stats of different types: {Type} and {other.Type}");
        return comparison(Value, other.Value);
    }

    public static bool operator >(Stat a, Stat b) => a.Compare(b, (x, y) => x > y);
    public static bool operator <(Stat a, Stat b) => a.Compare(b, (x, y) => x < y);
    public static bool operator >=(Stat a, Stat b) => !(a < b);
    public static bool operator <=(Stat a, Stat b) => !(a > b);

    public static bool operator >(Stat a, float b) => a.Value > b;
    public static bool operator <(Stat a, float b) => a.Value < b;
    public static bool operator >(float a, Stat b) => a > b.Value;
    public static bool operator <(float a, Stat b) => a < b.Value;
    public static bool operator >=(Stat a, float b) => !(a < b);
    public static bool operator <=(Stat a, float b) => !(a > b);
    public static bool operator >=(float a, Stat b) => !(a < b);
    public static bool operator <=(float a, Stat b) => !(a > b);
    public static bool operator >(Stat a, int b) => a.Value > b;
    public static bool operator <(Stat a, int b) => a.Value < b;
    public static bool operator >(int a, Stat b) => a > b.Value;
    public static bool operator <(int a, Stat b) => a < b.Value;
    public static bool operator >=(Stat a, int b) => !(a < b);
    public static bool operator <=(Stat a, int b) => !(a > b);
    public static bool operator >=(int a, Stat b) => !(a < b);
    public static bool operator <=(int a, Stat b) => !(a > b);

    public override string ToString()
    {
        return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
    }
}

