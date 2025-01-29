using System;

namespace StatSystem
{
    /// <summary>
    /// A Stat but with mutable properties, except for Type and Precision. Can be converted back to an immutable Stat. 
    /// </summary>
    public class MutableStat : IStat
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
        public MutableStat(IStat stat)
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

        public override string ToString()
        {
            return $"{Type}: {Value}{(Max.HasValue ? $"/{Max}" : "")}{(Min.HasValue ? $" (min {Min.Value})" : "")}";
        }
    }
}