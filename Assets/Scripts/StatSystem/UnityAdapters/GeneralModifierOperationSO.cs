using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Operation", menuName = "Stat System/Modifiers/Operations", order = 0)]
    public class GeneralModifierOperationSO : ModifierOperationSO
    {
        private class MyExampleModifierOperationFactory : ModifierOperationFactory
        {
            protected override OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                // Calculate regen value from queriedStats.
                // (Assuming queriedStats stores the latest HealthRegen value.)
                float regenValue = queriedStats[StatType.HealthRegen].Value;

                // OnTickUpdateDetails captures all variables in a closure
                return new OnTickUpdateDetails(stats =>
                {
                    stats[StatType.Health] += regenValue;
                }, stats =>
                {
                    stats[StatType.Strength] += 2 * currentTick;
                });
            }
        }

        public override ModifierOperationFactory Create()
        {
            return new MyExampleModifierOperationFactory();
        }
    }
}