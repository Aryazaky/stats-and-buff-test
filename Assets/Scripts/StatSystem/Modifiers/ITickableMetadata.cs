namespace StatSystem.Modifiers
{
    public interface ITickableMetadata
    {
        int TotalTicksElapsed { get; }
        bool HasUnprocessedTick { get; }
        float LastTickTime { get; }
        void MarkTickProcessed();
    }
}