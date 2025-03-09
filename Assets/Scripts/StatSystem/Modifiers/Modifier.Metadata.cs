namespace StatSystem.Modifiers
{
    public partial class Modifier
    {
        public readonly struct Metadata : IModifierMetadata, IAgeMetadata
        {
            private readonly Modifier _modifier;
            public Metadata(Modifier modifier)
            {
                _modifier = modifier;
            }

            public float Age => _modifier.Age;
            public int Priority => _modifier.Priority;
            public bool IsExpired => _modifier.IsExpired;
        }
    }
}