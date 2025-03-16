using StatSystem.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatSystem.UnityAdapters
{
    [CreateAssetMenu(fileName = "New Modifier Factory", menuName = "Stat System/Modifiers/Modifier", order = 0)]
    public class ModifierSO : ScriptableObject, IModifierFactory, IAssetMetadata
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private int priority;
        [FormerlySerializedAs("prerequisiteFactory")] [SerializeField] private ModifierActivePrerequisiteSO prerequisite;
        [FormerlySerializedAs("operationFactory")] [SerializeField] private ModifierOperationSO operation;
        
        public Modifier CreateModifier()
        {
            return new Modifier(operation.Create().Operation, priority, prerequisite.GetActivePrerequisite());
        }

        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
    }
}