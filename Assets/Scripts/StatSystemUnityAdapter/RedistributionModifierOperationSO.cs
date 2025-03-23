using System.Collections.Generic;
using System.Linq;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Redistribute Modifier Operation",
        menuName = "Stat System/Modifiers/Operations/Redistribute", order = 3)]
    public class RedistributionModifierOperationSO : ModifierOperationSO
    {
        [SerializeField] private StatType fromStat;
        [SerializeField] private StatTypeMultiplierPair[] toStats;
        [SerializeField, Min(1)] private int totalTickNeeded;
        [SerializeField] private bool enableLawOfEqualExchange;
        [SerializeField, Range(0, 1)] private float redistributionPercentage;
        [SerializeField] private bool useFlatRedistribution;
        [SerializeField] private float flatRedistributionValue;

        private class StatRedistributionFactory : ModifierOperationFactory
        {
            private readonly RedistributionModifierOperationSO _data;
            public StatRedistributionFactory(RedistributionModifierOperationSO data) => _data = data;

            protected override UpdateDetails CreateUpdateDetails(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                if (!queriedStats.TryGetStat(_data.fromStat, out var fromStat))
                    return new NoUpdateDetails();
    
                float totalFromValue = fromStat.Value;
                float tickFraction = 1f / _data.totalTickNeeded;

                // Check if we should use flat redistribution
                if (_data.useFlatRedistribution)
                {
                    // Use flat value redistribution
                    float flatValuePerTick = _data.flatRedistributionValue * tickFraction;

                    return new UpdateDetails(_data.applyToBaseStats, stats =>
                    {
                        foreach (var pair in _data.toStats)
                        {
                            float transferAmount = flatValuePerTick / _data.toStats.Length;
                            if (_data.enableLawOfEqualExchange)
                            {
                                transferAmount /= _data.toStats.Length;
                            }
                            stats.SafeEdit(pair.stat, stat => stat.Value += transferAmount);
                        }
                        stats.SafeEdit(_data.fromStat, stat => stat.Value -= flatValuePerTick);
                    });
                }
                else
                {
                    // Use percentage-based redistribution
                    float totalMultiplier = _data.toStats.Sum(pair => pair.multiplier);
                    return new UpdateDetails(_data.applyToBaseStats, stats =>
                    {
                        foreach (var pair in _data.toStats)
                        {
                            float transferAmount = totalFromValue * (pair.multiplier / totalMultiplier) * _data.redistributionPercentage * tickFraction;
                            if (_data.enableLawOfEqualExchange)
                            {
                                transferAmount /= _data.toStats.Length;
                            }
                            stats.SafeEdit(pair.stat, stat => stat.Value += transferAmount);
                        }
                        stats.SafeEdit(_data.fromStat, stat => stat.Value -= totalFromValue * _data.redistributionPercentage * tickFraction);
                    });
                }
            }

        }

        public override ModifierOperationFactory Create() => new StatRedistributionFactory(this);
    }
}