using StatSystem.Collections;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Operation", menuName = "Stat System/Modifiers/Operations", order = 0)]
    public class GeneralModifierOperationFactory : ModifierOperationFactory
    {
        protected override StatCollectionStruct ComputeOnTickStatCollection(ModifierOnTickOperationContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void Apply(IMutableStatCollection queriedStats, int currentTick)
        {
            throw new System.NotImplementedException();
        }
    }
}