using System.Collections.Generic;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Scale Modifier Operation", menuName = "Stat System/Modifiers/Operations/Scale",
        order = 1)]
    public class ScaleModifierOperationSO : ModifierOperationSO
    {
        [SerializeField] private StatType targetStat;
        [SerializeField] private List<StatTypeMultiplierPair> scalingStats;
        [SerializeField] private float flatBonus;
        [SerializeField] private float growthRate;

        private class StatScalingFactory : ModifierOperationFactory
        {
            private readonly ScaleModifierOperationSO _data;
            public StatScalingFactory(ScaleModifierOperationSO data) => _data = data;

            protected override UpdateDetails CreateUpdateDetails(int currentTick, ReadOnlyStatIndexer baseStats, ReadOnlyStatIndexer queriedStats)
            {
                float totalValue = _data.flatBonus + (_data.growthRate * currentTick);
                foreach (var scaling in _data.scalingStats)
                {
                    if (queriedStats.TryGetStat(scaling.stat, out var stat))
                    {
                        totalValue += stat.Value * scaling.multiplier;
                    }
                }

                return new UpdateDetails(_data.applyToBaseStats, stats =>
                    stats.SafeEdit(_data.targetStat, stat => stat.Value += totalValue));
            }
        }

        public override ModifierOperationFactory Create() => new StatScalingFactory(this);
    }
}