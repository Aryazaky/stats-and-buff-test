using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    public abstract partial class Modifier
    {
        public readonly struct Contexts
        {
            public readonly IQuery Query;
            public readonly IModifier ModifierMetadata;

            public Contexts(IQuery query, Modifier modifier)
            {
                Query = query;
                ModifierMetadata = modifier.ExtractMetadata();
            }
        }
    }
}