using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    public abstract class ModifierOperationSO : ScriptableObject
    {
        public abstract ModifierOperationFactory Create();
    }
}