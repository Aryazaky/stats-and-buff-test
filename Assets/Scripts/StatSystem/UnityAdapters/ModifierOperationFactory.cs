using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public abstract class ModifierOperationFactory : ScriptableObject
    {
        protected struct ModifierOnTickOperationContext
        {
            public StatCollectionStruct BaseStats { get; }
            public StatCollectionStruct DisplayedStats { get; }
            public int CurrentTick { get; }

            public ModifierOnTickOperationContext(StatCollectionStruct baseStats, StatCollectionStruct displayedStats, int currentTick)
            {
                BaseStats = baseStats;
                DisplayedStats = displayedStats;
                CurrentTick = currentTick;
            }
        }
        protected struct ModifierOperationContext
        {
            public StatCollectionStruct BaseStats { get; }
            public StatCollectionStruct DisplayedStats { get; }
            public int CurrentTick { get; }

            public ModifierOperationContext(StatCollectionStruct baseStats, StatCollectionStruct displayedStats, int currentTick)
            {
                BaseStats = baseStats;
                DisplayedStats = displayedStats;
                CurrentTick = currentTick;
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
                var statCollection = ComputeOnTickStatCollection(new ModifierOnTickOperationContext(baseStats, queriedStats, currentTick));
                foreach (var type in statCollection.Types)
                {
                    baseStats[type] = new MutableStat(statCollection[type]);
                    queriedStats[type] = new MutableStat(statCollection[type]);
                }
                modifierMetadata.MarkTickProcessed();
            }
        
            Apply(queriedStats, currentTick);
        }

        protected abstract StatCollectionStruct ComputeOnTickStatCollection(ModifierOnTickOperationContext context);

        protected abstract void Apply(IMutableStatCollection queriedStats, int currentTick);
    }
}