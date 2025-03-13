namespace StatSystem.Modifiers
{
    public readonly struct StatModifierMetadata : Modifier.IModifierMetadata, ITickableMetadata
    {
        private readonly TickableModifier _modifier;

        public StatModifierMetadata(TickableModifier modifier)
        {
            _modifier = modifier;
        }

        public int Priority => _modifier.Priority;
        public bool IsExpired => _modifier.IsExpired;

        public int TotalTicksElapsed => _modifier.TotalTicksElapsed;
        public bool HasUnprocessedTick => _modifier.HasUnprocessedTick;
        public float LastTickTime => _modifier.LastTickTime;
        public void MarkTickProcessed() => _modifier.MarkTickProcessed();
        public float Age => _modifier.Age;
    }
}