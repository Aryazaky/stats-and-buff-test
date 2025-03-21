using System;
using System.Linq;
using StatSystem.Collections;
using UnityEngine;

namespace StatSystemUnityAdapter
{
    [Serializable]
    public struct StatCollectionWrapper : IWrapper<StatCollectionStruct>
    {
        [SerializeField] private StatWrapper[] stats;

        public StatCollectionWrapper(StatCollectionStruct statCollection)
        {
            stats = statCollection.Select(stat => new StatWrapper(stat)).ToArray();
        }

        public void Update(StatCollectionStruct statCollection)
        {
            stats = statCollection.Select(stat => new StatWrapper(stat)).ToArray();
        }

        public StatCollectionStruct ToOriginal()
        {
            return new StatCollectionStruct(stats.Select(stat => stat.ToOriginal()));
        }
    }
}