using StatSystem;
using StatSystem.Collections;
using StatSystem.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatSystemUnityAdapter
{
    [CreateAssetMenu(fileName = "New Clamp Modifier Operation", menuName = "Stat System/Modifiers/Operations/Clamp", order = 2)]
    public class ClampModifierOperationSO : ModifierOperationSO
    {
        private enum ClampType
        {
            Fixed,
            StatBased,
            PercentageBased
        }

        [SerializeField] private StatType targetStat;
        [SerializeField] private ClampType mode;
        [Header("If fixed")]
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        [Header("If stat-based")]
        [SerializeField] private StatType minStat;
        [SerializeField] private StatType maxStat;
        [Header("If percentage-based")]
        [SerializeField] private StatType baseStat;
        [SerializeField] private float minMultiplier;
        [SerializeField] private float maxMultiplier;

        private class StatClampFactory : ModifierOperationFactory
        {
            private readonly ClampModifierOperationSO _data;
            public StatClampFactory(ClampModifierOperationSO data) => _data = data;

            protected override UpdateDetails CreateUpdateDetails(int currentTick, ReadOnlyStatIndexer baseStats,
                ReadOnlyStatIndexer queriedStats)
            {
                float min = _data.minValue, max = _data.maxValue;

                if (_data.mode == ClampType.StatBased)
                {
                    if (queriedStats.TryGetStat(_data.minStat, out var minStat)) min = minStat.Value;
                    if (queriedStats.TryGetStat(_data.maxStat, out var maxStat)) max = maxStat.Value;
                }
                else if (_data.mode == ClampType.PercentageBased &&
                         queriedStats.TryGetStat(_data.baseStat, out var baseStat))
                {
                    min = baseStat.Value * _data.minMultiplier;
                    max = baseStat.Value * _data.maxMultiplier;
                }

                return new UpdateDetails(_data.applyToBaseStats, stats =>
                    stats.SafeEdit(_data.targetStat, stat => stat.Value = Mathf.Clamp(stat.Value, min, max)));
            }
        }

        public override ModifierOperationFactory Create() => new StatClampFactory(this);
    }
}