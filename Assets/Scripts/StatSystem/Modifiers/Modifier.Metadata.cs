namespace StatSystem.Modifiers
{
    public partial class Modifier
    {
        private readonly struct ModifierMetadata : IModifierMetadata
        {
            private readonly Modifier _modifier;
            public ModifierMetadata(Modifier modifier)
            {
                _modifier = modifier;
            }

            public float Age => _modifier.Age;
            public int Priority => _modifier.Priority;
            public bool IsExpired => _modifier.IsExpired;
            public int TotalTicksElapsed => _modifier.TotalTicksElapsed;
            public bool HasUnprocessedTick => _modifier.HasUnprocessedTick;
            public float LastTickTime => _modifier.LastTickTime;
            public void MarkTickProcessed(UpdateDetails updateDetails) => _modifier.MarkTickProcessed(updateDetails);
            public UpdateDetails LastUpdateDetails => _modifier.LastUpdateDetails;
        }
    }
}