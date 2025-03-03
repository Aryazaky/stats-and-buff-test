namespace StatSystem.Modifiers
{
    public readonly struct StatMetadata : Modifier.IModifier, ITickableMetadata, IAgeMetadata
    {
        private readonly StatModifier _modifier;

        public StatMetadata(StatModifier modifier)
        {
            _modifier = modifier;
        }

        public float CreatedTime => _modifier.Age;
        public int Priority => _modifier.Priority;
        public int TotalTicksElapsed => _modifier.TotalTicksElapsed;
        public bool HasUnprocessedTick => _modifier.HasUnprocessedTick;
        public float LastTickTime => _modifier.LastTickTime;
        public void MarkTickProcessed() => _modifier.MarkTickProcessed();
        public float Age => _modifier.Age;
    }
}