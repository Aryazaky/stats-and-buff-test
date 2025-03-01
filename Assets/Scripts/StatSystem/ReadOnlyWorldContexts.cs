using System;
using System.Collections;
using System.Collections.Generic;

namespace StatSystem
{
    public class ReadOnlyWorldContexts : IReadOnlyWorldContexts
    {
        private readonly WorldContexts _worldContexts;
    
        public ReadOnlyWorldContexts(WorldContexts worldContexts) => _worldContexts = worldContexts;

        public IEnumerator<IWorldContext> GetEnumerator() => _worldContexts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_worldContexts).GetEnumerator();

        public int Count => _worldContexts.Count;
        
        public bool Contains(IWorldContext item) => _worldContexts.Contains(item);

        public bool Contains<T>(Func<T, bool> predicate = null) where T : IWorldContext => _worldContexts.Contains(predicate);
    }

    public interface IReadOnlyWorldContexts : IReadOnlyCollection<IWorldContext>
    {
        bool Contains(IWorldContext item);
        bool Contains<T>(Func<T, bool> predicate = null) where T : IWorldContext;
    }
}