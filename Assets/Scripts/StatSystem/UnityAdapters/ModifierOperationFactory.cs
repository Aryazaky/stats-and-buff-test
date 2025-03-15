using System;
using System.Collections.Generic;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public abstract class ModifierOperationFactory : ScriptableObject
    {
        protected class OnTickUpdateDetails
        {
            /// <summary>
            /// These actions will be applied to both baseStats and queriedStats.
            /// </summary>
            public List<Action<IMutableStatCollection>> SynchronizedUpdates { get; }
    
            /// <summary>
            /// These actions will be applied only to queriedStats.
            /// </summary>
            public List<Action<IMutableStatCollection>> NonSynchronizedUpdates { get; }

            public OnTickUpdateDetails(
                List<Action<IMutableStatCollection>> synchronizedUpdates,
                List<Action<IMutableStatCollection>> nonSynchronizedUpdates)
            {
                SynchronizedUpdates = synchronizedUpdates ?? new List<Action<IMutableStatCollection>>();
                NonSynchronizedUpdates = nonSynchronizedUpdates ?? new List<Action<IMutableStatCollection>>();
            }
        }
        
        public Modifier.Operation GetOperation() => Operation;

        private void Operation(Modifier.Contexts contexts)
        {
            var queriedStats = contexts.Query.QueriedStats;
            var baseStats = contexts.Query.BaseStats;
            var modifierMetadata = contexts.ModifierMetadata;
            var currentTick = modifierMetadata.TotalTicksElapsed;
        
            if (modifierMetadata.HasUnprocessedTick)
            {
                ApplyOnTickStatChangesTemplate(currentTick, baseStats, queriedStats);
                modifierMetadata.MarkTickProcessed();
            }
    
            ApplyTemporaryStatChanges(currentTick, new ReadOnlyStatIndexer(baseStats), queriedStats);
        }

        private void ApplyOnTickStatChangesTemplate(int currentTick, StatCollection baseStats, StatCollection queriedStats)
        {
            var updateDetails = CreateOnTickUpdate(currentTick, new ReadOnlyStatIndexer(baseStats), new ReadOnlyStatIndexer(queriedStats));
        
            // Apply synchronized updates to both collections.
            foreach (var update in updateDetails.SynchronizedUpdates)
            {
                update(baseStats);
                update(queriedStats);
            }
        
            // Apply non-synchronized updates only to queriedStats.
            foreach (var update in updateDetails.NonSynchronizedUpdates)
            {
                update(queriedStats);
            }
        }

        /// <summary>
        /// Subclasses provide the composite update details. 
        /// They can use both baseStats and queriedStats to decide what to update.
        /// </summary>
        protected abstract OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats);

        protected abstract void ApplyTemporaryStatChanges(int currentTick, ReadOnlyStatIndexer baseStats, IMutableStatCollection queriedStats);
    }
}