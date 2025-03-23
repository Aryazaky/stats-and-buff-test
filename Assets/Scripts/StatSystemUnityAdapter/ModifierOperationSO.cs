using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    public abstract class ModifierOperationSO : ScriptableObject
    {
        [SerializeField] protected bool applyToBaseStats;
        public abstract ModifierOperationFactory Create();
    }
}