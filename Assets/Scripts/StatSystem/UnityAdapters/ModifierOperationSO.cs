using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public abstract class ModifierOperationSO : ScriptableObject
    {
        public abstract ModifierOperationFactory Create();
    }
}