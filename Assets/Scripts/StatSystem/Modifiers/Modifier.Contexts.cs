using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        public readonly struct Contexts
        {
            public readonly Query Query;
            public readonly Metadata ModifierMetadata;

            public Contexts(Query query, Modifier modifier)
            {
                Query = query;
                ModifierMetadata = new Metadata(modifier);
            }
        }
    }
}