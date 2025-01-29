namespace StatSystem
{
    public readonly partial struct Stat
    {
        public abstract partial class Modifier
        {
            public readonly struct Contexts
            {
                public readonly StatQuery Query;
                public readonly Metadata ModifierMetadata;

                public Contexts(StatQuery query, Modifier modifier)
                {
                    Query = query;
                    ModifierMetadata = new Metadata(modifier);
                }
            }
        }
    }
}