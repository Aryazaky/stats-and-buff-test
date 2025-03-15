using System.Collections.Generic;

namespace StatSystem.Collections
{
    public interface IBaseStatCollection
    {
        IEnumerable<StatType> Types { get; }
        bool Contains(params StatType[] type);
    }
}