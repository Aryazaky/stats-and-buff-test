using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public abstract class ModifierOperationFactory : ScriptableObject
    {
        protected struct ModifierTickContext
        {
            public StatCollectionStruct BaseStats { get; }
            public StatCollectionStruct DisplayedStats { get; }
            public int CurrentTick { get; }

            public ModifierTickContext(StatCollectionStruct baseStats, StatCollectionStruct displayedStats, int currentTick)
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
            int? currentTick = null;
        
            if (contexts.ModifierMetadata is ITickableMetadata tickableMetadata)
            {
                currentTick = tickableMetadata.TotalTicksElapsed;
                if (tickableMetadata.HasUnprocessedTick)
                {
                    ComputeStatCollection(new(baseStats, queriedStats, currentTick.Value), out var statCollection);
                    foreach (var type in statCollection.Types)
                    {
                        baseStats[type] = new MutableStat(statCollection[type]);
                        queriedStats[type] = new MutableStat(statCollection[type]);
                    }
                    tickableMetadata.MarkTickProcessed();
                }
            }
        
            Apply(queriedStats, currentTick);
        }

        protected abstract void ComputeStatCollection(ModifierTickContext context, out StatCollectionStruct final);

        protected abstract void Apply(IMutableStatCollection queriedStats, int? currentTick);
    }
}