using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Modifier Operation", menuName = "Stat System/Modifiers/Operations/Regen", order = 0)]
    public class GeneralModifierOperationSO : ModifierOperationSO
    {
        private class MyExampleModifierOperationFactory : ModifierOperationFactory
        {
            protected override UpdateDetails CreateUpdateDetails(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                // Calculate regen value from queriedStats.
                float regenValue = 0;
                if (queriedStats.TryGetStat(StatType.HealthRegen, out var regenStat))
                {
                    regenValue = regenStat.Value;
                }

                // OnTickUpdateDetails captures all variables in a closure
                return new UpdateDetails(syncedUpdate: stats =>
                {
                    // Affects base stats too
                    stats.SafeEdit(StatType.Health, stat => stat.Value += regenValue);
                }, nonSyncedUpdate: stats =>
                {
                    // Affects only the end result stat
                    stats.SafeEdit(StatType.Strength, stat => stat.Value += 2 * currentTick);
                });
            }
        }

        public override ModifierOperationFactory Create() => new MyExampleModifierOperationFactory();
    }
}