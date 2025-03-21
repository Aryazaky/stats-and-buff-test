using UnityEngine;

namespace StatSystemUnityAdapter
{
    public interface IAssetMetadata
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
    }
}