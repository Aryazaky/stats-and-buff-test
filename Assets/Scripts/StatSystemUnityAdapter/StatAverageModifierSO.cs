using System.Collections.Generic;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Average Modifier Operation", menuName = "Stat System/Modifiers/Operations/Average",
        order = 4)]
    public class StatAverageModifierSO : ModifierOperationSO
    {
        [SerializeField] private StatType targetStat;
        [SerializeField] private List<StatType> stats;
        [SerializeField] private float multiplier = 1;

        private class StatAverageFactory : ModifierOperationFactory
        {
            private readonly StatAverageModifierSO _data;
            public StatAverageFactory(StatAverageModifierSO data) => _data = data;

            protected override UpdateDetails CreateUpdateDetails(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats)
            {
                float sum = 0;
                int count = 0;
                foreach (var statType in _data.stats)
                {
                    if (queriedStats.TryGetStat(statType, out var stat))
                    {
                        sum += stat.Value;
                        count++;
                    }
                }

                float avg = count > 0 ? (sum / count) * _data.multiplier : 0;
                return new UpdateDetails(_data.applyToBaseStats, stats =>
                    stats.SafeEdit(_data.targetStat, stat => stat.Value += avg));
            }
        }

        public override ModifierOperationFactory Create() => new StatAverageFactory(this);
    }
}