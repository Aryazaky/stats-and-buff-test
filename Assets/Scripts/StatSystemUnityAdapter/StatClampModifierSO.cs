using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Stat Clamp Modifier", menuName = "Stat System/Modifiers/Stat Clamping", order = 2)]
    public class StatClampModifierSO : ModifierOperationSO
    {
        public enum ClampType
        {
            Fixed,
            StatBased,
            PercentageBased
        }

        public StatType TargetStat;
        public ClampType Mode;
        public float MinValue, MaxValue;
        public StatType MinStat, MaxStat, BaseStat;
        public float MinMultiplier, MaxMultiplier;

        private class StatClampFactory : ModifierOperationFactory
        {
            private readonly StatClampModifierSO _data;
            public StatClampFactory(StatClampModifierSO data) => _data = data;

            protected override OnTickUpdateDetails CreateOnTickUpdate(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                float min = _data.MinValue, max = _data.MaxValue;

                if (_data.Mode == ClampType.StatBased)
                {
                    if (queriedStats.TryGetStat(_data.MinStat, out var minStat)) min = minStat.Value;
                    if (queriedStats.TryGetStat(_data.MaxStat, out var maxStat)) max = maxStat.Value;
                }
                else if (_data.Mode == ClampType.PercentageBased &&
                         queriedStats.TryGetStat(_data.BaseStat, out var baseStat))
                {
                    min = baseStat.Value * _data.MinMultiplier;
                    max = baseStat.Value * _data.MaxMultiplier;
                }

                return new OnTickUpdateDetails(stats =>
                    stats.SafeEdit(_data.TargetStat, stat => stat.Value = Mathf.Clamp(stat.Value, min, max)));
            }
        }

        public override ModifierOperationFactory Create() => new StatClampFactory(this);
    }
}