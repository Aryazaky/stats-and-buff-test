namespace StatSystem
{
    public readonly partial struct Stat
    {
        public abstract partial class Modifier
        {
            public readonly struct Metadata : IModifier
            {
                private readonly Modifier _modifier;
                public Metadata(Modifier modifier)
                {
                    _modifier = modifier;
                }

                public float LastInvokeTime => _modifier.LastInvokeTime;
                public float CreatedTime => _modifier.CreatedTime;
                public int InvokedCount => _modifier.InvokedCount;
                public int Priority => _modifier.Priority;
            }
        }
    }
}