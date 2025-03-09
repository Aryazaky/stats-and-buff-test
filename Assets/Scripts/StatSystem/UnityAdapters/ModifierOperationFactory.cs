using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Operation", menuName = "Stat System/Modifiers/Operations", order = 0)]
    public class ModifierOperationFactory : ScriptableObject
    {
        public Modifier.Operation GetOperation() => Operation;

        protected virtual void Operation(Modifier.Contexts contexts)
        {
            throw new System.NotImplementedException();
        }
    }
}