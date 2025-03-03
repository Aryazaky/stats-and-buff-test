namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        public readonly struct Metadata : IModifier, IAgeMetadata
        {
            private readonly Modifier _modifier;
            public Metadata(Modifier modifier)
            {
                _modifier = modifier;
            }

            public float Age => _modifier.Age;
            public int Priority => _modifier.Priority;
        }
    }
}