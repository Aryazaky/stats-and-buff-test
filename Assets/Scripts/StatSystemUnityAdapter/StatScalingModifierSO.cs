using System.Collections.Generic;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Stat Scaling Modifier", menuName = "Stat System/Modifiers/Stat Scaling",
        order = 1)]
    public class StatScalingModifierSO : ModifierOperationSO
    {
        [System.Serializable]
        public struct StatMultiplierPair
        {
            public StatType Stat;
            public float Multiplier;
        }

        public StatType TargetStat;
        public List<StatMultiplierPair> ScalingStats;
        public float FlatBonus;

        private class StatScalingFactory : ModifierOperationFactory
        {
            private readonly StatScalingModifierSO _data;
            public StatScalingFactory(StatScalingModifierSO data) => _data = data;

            protected override OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                float totalValue = _data.FlatBonus;
                foreach (var scaling in _data.ScalingStats)
                {
                    if (queriedStats.TryGetStat(scaling.Stat, out var stat))
                    {
                        totalValue += stat.Value * scaling.Multiplier;
                    }
                }

                return new OnTickUpdateDetails(stats =>
                    stats.SafeEdit(_data.TargetStat, stat => stat.Value += totalValue));
            }
        }

        public override ModifierOperationFactory Create() => new StatScalingFactory(this);
    }
}