using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public interface IAssetMetadata
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
    }
}