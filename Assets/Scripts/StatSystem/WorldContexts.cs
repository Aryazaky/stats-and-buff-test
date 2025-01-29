using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatSystem
{
    public class WorldContexts : ICollection<IWorldContext>
    {
        private readonly HashSet<IWorldContext> _activeContexts;

        public IEnumerable<IWorldContext> ActiveContexts => _activeContexts;
        
        public WorldContexts(params IWorldContext[] contexts)
        {
            _activeContexts = contexts.ToHashSet();
        }

        public IEnumerator<IWorldContext> GetEnumerator()
        {
            return _activeContexts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_activeContexts).GetEnumerator();
        }

        public void Add(IWorldContext item)
        {
            _activeContexts.Add(item);
        }

        public void Clear()
        {
            _activeContexts.Clear();
        }

        public bool Contains(IWorldContext item)
        {
            return _activeContexts.Contains(item);
        }

        public bool Contains<T>() where T : IWorldContext
        {
            return _activeContexts.OfType<T>().Any();
        }

        public void CopyTo(IWorldContext[] array, int arrayIndex)
        {
            _activeContexts.CopyTo(array, arrayIndex);
        }

        public bool Remove(IWorldContext item)
        {
            return _activeContexts.Remove(item);
        }

        public int Count => _activeContexts.Count;

        public bool IsReadOnly => ((ICollection<IWorldContext>)_activeContexts).IsReadOnly;
    }

    public class ExampleIsRaining : IWorldContext
    {
        public string Name => "Hungry player";
    }
}