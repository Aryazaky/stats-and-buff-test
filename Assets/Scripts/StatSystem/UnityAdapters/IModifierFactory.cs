using StatSystem.Modifiers;

namespace StatSystem.UnityAdapters
{
    public interface IModifierFactory : IAssetMetadata
    {
        Modifier CreateModifier();
    }
}