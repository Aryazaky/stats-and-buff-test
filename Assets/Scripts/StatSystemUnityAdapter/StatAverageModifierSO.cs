using System.Collections.Generic;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Stat Average Modifier", menuName = "Stat System/Modifiers/Stat Average",
        order = 4)]
    public class StatAverageModifierSO : ModifierOperationSO
    {
        public StatType TargetStat;
        public List<StatType> Stats;
        public float Multiplier;

        private class StatAverageFactory : ModifierOperationFactory
        {
            private readonly StatAverageModifierSO _data;
            public StatAverageFactory(StatAverageModifierSO data) => _data = data;

            protected override OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                float sum = 0;
                int count = 0;
                foreach (var statType in _data.Stats)
                {
                    if (queriedStats.TryGetStat(statType, out var stat))
                    {
                        sum += stat.Value;
                        count++;
                    }
                }

                float avg = count > 0 ? (sum / count) * _data.Multiplier : 0;

                return new OnTickUpdateDetails(stats => stats.SafeEdit(_data.TargetStat, stat => stat.Value += avg));
            }
        }

        public override ModifierOperationFactory Create() => new StatAverageFactory(this);
    }
}