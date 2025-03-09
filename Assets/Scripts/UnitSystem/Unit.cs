using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] ModifierFactory factory;
        [SerializeField] ModifierWrapper[] modifiers;

        private void Start()
        {
            _stats = new Stats(statCollection.ToOriginal());
            _stats.Mediator.AddModifier(factory.CreateModifier());
            _stats.Mediator.AddModifier(factory.CreateModifier());
        }

        void Update()
        {
            _stats[StatType.Health] += 0.001f;
            statCollection.Update(_stats.Snapshot());
            modifiers = _stats.Mediator.Select(modifier => new ModifierWrapper(modifier, factory)).ToArray();
        }

        private void OnValidate()
        {
            _stats = new Stats(statCollection.ToOriginal());
        }

        public IMutableStatIndexer Stats => _stats;
    }
}
