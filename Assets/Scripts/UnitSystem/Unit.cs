using System;
using System.Collections.Generic;
using System.Linq;
using StatSystem;
using StatSystem.Collections;
using StatSystemUnityAdapter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        private Stats _stats;
        private WorldContexts _worldContexts;
        [SerializeField] StatCollectionWrapper statCollection;
        [SerializeField] ModifierSO factory;
        [SerializeField] ModifierWrapper[] modifiers;
        [SerializeField] PlayerInput playerInput;

        private void Start()
        {
            _worldContexts = new WorldContexts();
            _stats = new Stats(statCollection.ToOriginal());
            _stats.Mediator.AddModifier(factory.CreateModifier());
            _stats.Mediator.AddModifier(factory.CreateModifier());
        }

        private void OnEnable()
        {
            playerInput.actions.FindAction("Jump").performed += OnInteract;
        }

        private void OnDisable()
        {
            playerInput.actions.FindAction("Jump").performed -= OnInteract;
        }

        void Update()
        {
            statCollection.Update(_stats.Snapshot());
            modifiers = _stats.Mediator.Select(modifier => new ModifierWrapper(modifier, factory)).ToArray();
        }

        private void OnValidate()
        {
            _stats = new Stats(statCollection.ToOriginal(), _stats?.Mediator);
        }

        public ReadOnlyStatIndexer Stats => new(_stats.Snapshot());
        
        private void OnInteract(InputAction.CallbackContext obj)
        {
            _stats.Update(_worldContexts);
        }
    }
}
