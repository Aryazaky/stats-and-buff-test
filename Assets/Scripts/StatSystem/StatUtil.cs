using System;

namespace StatSystem
{
    public static class StatUtil
    {
        public static T ConvertTo<T>(this IStat stat) where T : IStat
        {
            return stat switch
            {
                T correctStat => correctStat,
                Stat s when typeof(T) == typeof(MutableStat) => (T)(object)new MutableStat(s),
                MutableStat s when typeof(T) == typeof(Stat) => (T)(object)(Stat)s,
                _ => (T)Activator.CreateInstance(typeof(T), stat.Type, stat.Value, stat.Min, stat.Max, stat.Precision)
            };
        }
    }
}