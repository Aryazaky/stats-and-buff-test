using System;
using System.Collections.Generic;

namespace StatSystem
{
    public interface IReadOnlyWorldContexts : IReadOnlyCollection<IWorldContext>
    {
        bool Contains(IWorldContext item);
        bool Contains<T>(Func<T, bool> predicate = null) where T : IWorldContext;
    }
}