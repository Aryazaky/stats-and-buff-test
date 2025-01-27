namespace StatSystem
{
    public readonly partial struct Stat
    {
        public abstract partial class Modifier
        {
            public readonly struct Contexts
            {
                public readonly QueryArgs QueryArgs;
                public readonly Metadata ModifierMetadata;

                public Contexts(QueryArgs queryArgs, Modifier modifier)
                {
                    QueryArgs = queryArgs;
                    ModifierMetadata = new Metadata(modifier);
                }
            }
        }
    }
}