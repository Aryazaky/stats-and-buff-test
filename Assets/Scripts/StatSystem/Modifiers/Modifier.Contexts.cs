using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    public partial class Modifier
    {
        public readonly struct Contexts
        {
            public readonly IQuery Query;
            public readonly IModifierMetadata ModifierMetadata;

            public Contexts(IQuery query, Modifier modifier)
            {
                Query = query;
                ModifierMetadata = modifier.ExtractMetadata();
            }
        }
    }
}