using System;
using StatSystem;
using StatSystem.Collections;
using StatSystem.UnityAdapters;
using UnityEngine;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        private Stats _stats;
        [SerializeField] StatCollectionWrapper statCollection;

        void Update()
        {
            _stats[StatType.Health] += 0.001f;
            statCollection.Update(_stats.Snapshot());
        }

        private void OnValidate()
        {
            _stats = new Stats(statCollection.ToOriginal());
        }

        public IMutableStatIndexer Stats => _stats;
    }
}
