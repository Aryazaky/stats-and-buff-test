using System;
using System.Collections.Generic;
using StatSystem.Collections;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Operation", menuName = "Stat System/Modifiers/Operations", order = 0)]
    public class GeneralModifierOperationFactory : ModifierOperationFactory
    {
        protected override OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats)
        {
            var syncUpdates = new List<Action<IMutableStatCollection>>();
            var nonSyncUpdates = new List<Action<IMutableStatCollection>>();

            // Calculate regen value from queriedStats.
            // (Assuming queriedStats stores the latest HealthRegen value.)
            float regenValue = queriedStats[StatType.HealthRegen].Value;

            // Synchronized update: Regen Health in both collections.
            syncUpdates.Add(stats =>
            {
                stats[StatType.Health] += regenValue;
            });

            // Non-synchronized update: Increase Strength only in queriedStats.
            nonSyncUpdates.Add(stats =>
            {
                stats[StatType.Strength] += 2 * currentTick;
            });

            return new OnTickUpdateDetails(syncUpdates, nonSyncUpdates);
        }

        protected override void ApplyTemporaryStatChanges(int currentTick, ReadOnlyStatIndexer baseStats, IMutableStatCollection queriedStats)
        {
            // Temporary buff: Increase Defense based on initial base defense.
            queriedStats[StatType.Defense] += baseStats[StatType.Defense].Value * 0.5f;
        }
    }
}