using System;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [Serializable]
    public struct StatWrapper : IWrapper<Stat>
    {
        [SerializeField] private StatType type;
        [SerializeField] private float value;

        [SerializeField] private bool hasMin;
        [SerializeField] private float min;

        [SerializeField] private bool hasMax;
        [SerializeField] private float max;

        [SerializeField] private int precision;

        // Constructor to initialize from Stat
        public StatWrapper(Stat stat)
        {
            type = stat.Type;
            value = stat.Value;

            hasMin = stat.Min.HasValue;
            min = stat.Min ?? 0f;

            hasMax = stat.Max.HasValue;
            max = stat.Max ?? 0f;

            precision = stat.Precision;
        }

        public void Update(Stat stat)
        {
            type = stat.Type;
            value = stat.Value;

            hasMin = stat.Min.HasValue;
            min = stat.Min ?? 0f;

            hasMax = stat.Max.HasValue;
            max = stat.Max ?? 0f;

            precision = stat.Precision;
        }

        public Stat ToOriginal()
        {
            return new Stat(type, value, hasMin ? min : null, hasMax ? max : null, precision);
        }
    }
}