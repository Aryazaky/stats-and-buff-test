using System;
using System.Collections.Generic;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public abstract class ModifierOperationFactory : ScriptableObject
    {
        public Modifier.Operation GetOperation() => Operation;

        private void Operation(Modifier.Contexts contexts)
        {
            var queriedStats = contexts.Query.QueriedStats;
            var baseStats = contexts.Query.BaseStats;
            var modifierMetadata = contexts.ModifierMetadata;
            var currentTick = modifierMetadata.TotalTicksElapsed;
            var updateDetails = CreateOnTickUpdate(currentTick, new ReadOnlyStatIndexer(baseStats), new ReadOnlyStatIndexer(queriedStats));
        
            if (modifierMetadata.HasUnprocessedTick)
            {
                updateDetails.SynchronizedUpdate?.Invoke(baseStats);
                modifierMetadata.MarkTickProcessed(updateDetails);
            }
            // We assume the user is just querying some stuff between ticks.
            // To make it consistent, we saved the update details and invoke the action only to the queriedStats
            else if (modifierMetadata.LastUpdateDetails != null) 
            {
                updateDetails = modifierMetadata.LastUpdateDetails;
            }
            updateDetails.SynchronizedUpdate?.Invoke(queriedStats);
            updateDetails.NonSynchronizedUpdate?.Invoke(queriedStats);
        }

        /// <summary>
        /// Subclasses provide the composite update details. 
        /// They can use both baseStats and queriedStats to decide what to update.
        /// </summary>
        protected abstract OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats);
    }
}