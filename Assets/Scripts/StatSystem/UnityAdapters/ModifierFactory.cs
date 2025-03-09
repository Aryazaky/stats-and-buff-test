using System;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Factory", menuName = "Stat System/Modifiers/Modifier", order = 0)]
    public class ModifierFactory : ScriptableObject, IModifierFactory
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private int priority;
        [SerializeField] private ModifierActivePrerequisiteFactory prerequisiteFactory;
        [SerializeField] private ModifierOperationFactory operationFactory;
        
        public Modifier CreateModifier()
        {
            return new Modifier(operationFactory.GetOperation(), priority, prerequisiteFactory.GetActivePrerequisite());
        }

        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
    }
}