using System.Collections.Generic;
using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Stat Redistribution Modifier",
        menuName = "Stat System/Modifiers/Stat Redistribution", order = 3)]
    public class StatRedistributionModifierSO : ModifierOperationSO
    {
        public StatType FromStat;
        public List<StatScalingModifierSO.StatMultiplierPair> ToStats;

        private class StatRedistributionFactory : ModifierOperationFactory
        {
            private readonly StatRedistributionModifierSO _data;
            public StatRedistributionFactory(StatRedistributionModifierSO data) => _data = data;

            protected override OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                if (!queriedStats.TryGetStat(_data.FromStat, out var fromStat))
                    return new OnTickUpdateDetails(stats => { });
                float totalFromValue = fromStat.Value;
                float totalMultiplier = 0;

                foreach (var pair in _data.ToStats)
                    totalMultiplier += pair.Multiplier;

                return new OnTickUpdateDetails(stats =>
                {
                    foreach (var pair in _data.ToStats)
                        stats.SafeEdit(pair.Stat,
                            stat => stat.Value += (totalFromValue * (pair.Multiplier / totalMultiplier)));
                    stats.SafeEdit(_data.FromStat, stat => stat.Value -= totalFromValue);
                });
            }
        }

        public override ModifierOperationFactory Create() => new StatRedistributionFactory(this);
    }
}