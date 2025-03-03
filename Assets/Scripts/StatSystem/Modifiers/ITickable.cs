namespace StatSystem.Modifiers
{
    public interface ITickable : ITickableMetadata
    {
        void Tick();
    }
}