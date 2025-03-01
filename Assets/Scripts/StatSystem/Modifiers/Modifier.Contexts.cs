using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        public readonly struct Contexts
        {
            public readonly IQuery Query;
            public readonly Metadata ModifierMetadata;

            public Contexts(IQuery query, Modifier modifier)
            {
                Query = query;
                ModifierMetadata = new Metadata(modifier);
            }
        }
    }
}