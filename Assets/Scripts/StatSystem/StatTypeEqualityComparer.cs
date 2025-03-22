using System.Collections.Generic;

namespace StatSystem
{
    public class StatTypeEqualityComparer : IEqualityComparer<Stat>
    {
        public bool Equals(Stat x, Stat y)
        {
            return x.Type == y.Type;
        }

        public int GetHashCode(Stat obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}