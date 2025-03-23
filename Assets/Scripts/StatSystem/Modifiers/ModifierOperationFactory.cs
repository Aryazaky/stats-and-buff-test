using StatSystem.Collections;

namespace StatSystem.Modifiers
{
    public abstract class ModifierOperationFactory
    {
        public Modifier.Operation Operation => InternalOperation;

        private void InternalOperation(Modifier.Contexts contexts)
        {
            var queriedStats = contexts.Query.QueriedStats;
            var baseStats = contexts.Query.BaseStats;
            var modifierMetadata = contexts.Metadata;
            int currentTick = modifierMetadata.TotalTicksElapsed;
            var updateDetails = CreateUpdateDetails(currentTick, new ReadOnlyStatIndexer(baseStats), new ReadOnlyStatIndexer(queriedStats));

            if (modifierMetadata.HasUnprocessedTick)
            {
                updateDetails.SyncedUpdate?.Invoke(baseStats);
                modifierMetadata.MarkTickProcessed(updateDetails);
            }
            else if (modifierMetadata.LastUpdateDetails != null)
            {
                updateDetails = modifierMetadata.LastUpdateDetails;
            }

            updateDetails.SyncedUpdate?.Invoke(queriedStats);
            updateDetails.NonSyncedUpdate?.Invoke(queriedStats);
        }

        protected abstract UpdateDetails CreateUpdateDetails(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats);
    }
}