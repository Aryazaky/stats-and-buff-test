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
            set => _value = ValidateValue(value);
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
                    else
                    {
                        _min = value;
                        _value = ValidateValue(_value);
                    }
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
                        throw new Exception($"Max value ({value}) cannot be less than Min value ({_min}).");
                    }
                    else
                    {
                        _max = value;
                        _value = ValidateValue(_value);
                    }
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
                if (value is < 0 or > 15)
                {
                    throw new Exception($"Decimal places precision ({value}) must be between 0 and 15.");
                }
                _precision = value;
                Value = Value;
            }
        }

        /// <summary>
        /// Converts a Stat into a ModifiableStat class. 
        /// </summary>
        public MutableStat(Stat stat) : this(stat.Type, stat.Value, stat.Min, stat.Max, stat.Precision)
        {
        }

        public MutableStat(StatType type, float value, float? min = 0, float? max = null, int precision = 0)
        {
            Type = type;
            Precision = precision;
            Max = max;
            Min = min;
            Value = value;
        }

        private float ValidateValue(float value)
        {
            value = _max.HasValue ? MathF.Min(_max.Value, value) : value;
            value = _min.HasValue ? MathF.Max(_min.Value, value) : value;
            return MathF.Round(value, _precision);
        }

        public MutableStat Clone()
        {
            return new MutableStat(Type, _value, _min, _max, _precision);
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