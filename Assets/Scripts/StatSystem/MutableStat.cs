using System;

namespace StatSystem
{
    /// <summary>
    /// A Stat but with mutable properties, except for Type and Precision. Can be converted back to an immutable Stat. 
    /// </summary>
    public class MutableStat
    {
        private float _value;
        private float? _min;
        private float? _max;
        private int _precision;
        
        public StatType Type { get; }

        public float Value
        {
            get => _value;
            set {
                value = _max.HasValue ? MathF.Min(_max.Value, value) : value;
                value = _min.HasValue ? MathF.Max(_min.Value, value) : value;
                _value = MathF.Round(value, _precision);
            }
        }

        public float? Min
        {
            get => _min;
            set
            {
                if(value.HasValue)
                {
                    if (value > _max)
                    {
                        throw new Exception($"Min value ({value}) cannot be more than Max value ({_max}).");
                    }
                    else _min = value;
                }
                else
                {
                    _min = null;
                }
            }
        }

        public float? Max
        {
            get => _max;
            set
            {
                if(value.HasValue)
                {
                    if (value < _min)
                    {
                        throw new Exception($"Max value ({value}) cannot be more than Min value ({_min}).");
                    }
                    else _max = value;
                }
                else
                {
                    _max = null;
                }
            }
        }

        public int Precision
        {
            get => _precision;
            set
            {
                if (value < 0)
                {
                    throw new Exception($"Decimal places precision ({value}) cannot be negative.");
                }
                _precision = value;
            }
        }

        /// <summary>
        /// Converts a Stat into a ModifiableStat class. 
        /// </summary>
        public MutableStat(Stat stat)
        {
            Type = stat.Type;
            Min = stat.Min;
            Max = stat.Max;
            Precision = stat.Precision;
            Value = stat.Value;
        }

        public MutableStat(StatType type, float value, float? min = 0, float? max = null, int precision = 0)
        {
            Type = type;
            Precision = precision;
            Max = max;
            Min = min;
            Value = value;
        }

        public static implicit operator Stat(MutableStat stat)
        {
            return new Stat(type: stat.Type, value: stat.Value, min: stat.Min, max: stat.Max, precision: stat.Precision);
        }
        
        private MutableStat PerformOperation(Stat other, Func<float, float, float> operation)
        {
            if (Type != other.Type)
                throw new InvalidOperationException($"Cannot operate on Stats of different types: {Type} and {other.Type}");

            Value = operation(Value, other.Value);
            return this;
        }

        private MutableStat PerformOperation(float value, Func<float, float, float> operation)
        {
            Value = operation(Value, value);
            return this;
        }
        
        public static MutableStat operator +(MutableStat a, MutableStat b) => a.PerformOperation(b, (x, y) => x + y);
        public static MutableStat operator -(MutableStat a, MutableStat b) => a.PerformOperation(b, (x, y) => x - y);
        public static MutableStat operator +(MutableStat stat, float value) => stat.PerformOperation(value, (x, y) => x + y);
        public static MutableStat operator +(float value, MutableStat stat) => stat + value;
        public static MutableStat operator -(MutableStat stat, float value) => stat.PerformOperation(value, (x, y) => x - y);
        public static MutableStat operator -(float value, MutableStat stat) => stat.PerformOperation(value, (x, y) => y - x);
        public static MutableStat operator *(MutableStat stat, float value) => stat.PerformOperation(value, (x, y) => x * y);
        public static MutableStat operator *(float value, MutableStat stat) => stat * value;
        public static MutableStat operator /(MutableStat stat, float value) => stat.PerformOperation(value, (x, y) => x / y);
        public static MutableStat operator /(float value, MutableStat stat) => stat.PerformOperation(value, (x, y) => y / x);

        public override string ToString()
        {
            return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
        }
    }
}