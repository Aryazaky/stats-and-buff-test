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
            var updateDetails = CreateOnTickUpdate(currentTick, new ReadOnlyStatIndexer(baseStats), new ReadOnlyStatIndexer(queriedStats));

            if (modifierMetadata.HasUnprocessedTick)
            {
                updateDetails.SynchronizedUpdate?.Invoke(baseStats);
                modifierMetadata.MarkTickProcessed(updateDetails);
            }
            else if (modifierMetadata.LastUpdateDetails != null)
            {
                updateDetails = modifierMetadata.LastUpdateDetails;
            }

            updateDetails.SynchronizedUpdate?.Invoke(queriedStats);
            updateDetails.NonSynchronizedUpdate?.Invoke(queriedStats);
        }

        protected abstract OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats);
    }
}