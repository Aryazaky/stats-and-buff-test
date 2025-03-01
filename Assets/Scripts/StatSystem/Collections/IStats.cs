using StatSystem.Modifiers;

namespace StatSystem.Collections
{
    public interface IStats
    {
        Mediator Mediator { get; }
        bool IsDirty { get; }
        void Bake();
        void Update(IReadOnlyWorldContexts worldContexts, params StatType[] types);
    }
}