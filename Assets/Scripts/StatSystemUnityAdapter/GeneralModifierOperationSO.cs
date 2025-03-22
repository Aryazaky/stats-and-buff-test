using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
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
                float regenValue = 0;
                if (queriedStats.TryGetStat(StatType.HealthRegen, out var regenStat))
                {
                    regenValue = regenStat.Value;
                }

                // OnTickUpdateDetails captures all variables in a closure
                return new OnTickUpdateDetails(stats =>
                {
                    // Affects base stats too
                    stats.SafeEdit(StatType.Health, stat => stat.Value += regenValue);
                }, stats =>
                {
                    // Affects only the end result stat
                    stats.SafeEdit(StatType.Strength, stat => stat.Value += 2 * currentTick);
                });
            }
        }

        public override ModifierOperationFactory Create()
        {
            return new MyExampleModifierOperationFactory();
        }
    }
}