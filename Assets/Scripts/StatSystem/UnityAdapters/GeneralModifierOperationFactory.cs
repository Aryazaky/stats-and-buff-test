using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Operation", menuName = "Stat System/Modifiers/Operations", order = 0)]
    public class GeneralModifierOperationFactory : ModifierOperationFactory
    {
        public static void Regen(Modifier.Contexts contexts)
        {
            var displayedStats = contexts.Query.QueriedStats; // Temporary stats (used for displaying modified values)
            var baseStats = contexts.Query.BaseStats; // Permanent stats (updated across ticks)

            if (contexts.ModifierMetadata is ITickableMetadata { HasUnprocessedTick: true } tickableMetadata)
            {
                var regenValue = baseStats[StatType.HealthRegen].Value; // Warning: it's unknown if the stats has a HealthRegen property. Use baseStats.TryGetStat(StatType.HealthRegen, out var stat) instead

                // Update both reference and temporary stats for lasting effects
                baseStats[StatType.Health] += regenValue; // Warning: same as the warning above. 
                displayedStats[StatType.Health] += regenValue;

                tickableMetadata.MarkTickProcessed(); // Prevents multiple applications within the same tick
            }
        }

        protected override void ComputeStatCollection(ModifierTickContext context, out StatCollectionStruct final)
        {
            final = context.BaseStats;
            throw new System.NotImplementedException();
        }

        protected override void Apply(IMutableStatCollection queriedStats, int? currentTick)
        {
            throw new System.NotImplementedException();
        }
    }
}