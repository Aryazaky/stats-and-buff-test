using System;

namespace StatSystem.UnityAdapter
{
    [Serializable]
    public class StatWrapper
    {
        public StatType Type;
        public float Value;

        public bool HasMin;
        public float Min;

        public bool HasMax;
        public float Max;

        public int Precision;

        // Constructor to initialize from Stat
        public StatWrapper(Stat stat)
        {
            Type = stat.Type;
            Value = stat.Value;

            HasMin = stat.Min.HasValue;
            Min = stat.Min ?? 0f;

            HasMax = stat.Max.HasValue;
            Max = stat.Max ?? 0f;

            Precision = stat.Precision;
        }

        // Converts back to Stat struct (if needed)
        public Stat ToStat()
        {
            return new Stat(Type, Value, HasMin ? Min : (float?)null, HasMax ? Max : (float?)null, Precision);
        }
    }
}