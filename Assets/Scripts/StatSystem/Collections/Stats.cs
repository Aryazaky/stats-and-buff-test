using System;
using System.Collections.Generic;
using StatSystem.Collections.Generic;

namespace StatSystem.Collections
{
    [Serializable]
    public class Stats : Stats<Stat>
    {
        public Stats(params Stat[] stats) : base(stats)
        {
        }
    
        public Stats(IEnumerable<Stat> stats) : base(stats)
        {
        }
    }
}