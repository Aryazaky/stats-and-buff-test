using StatSystem.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatSystemUnityAdapter
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
            return new Modifier(operation.Create().Operation, priority, prerequisite.GetActivePrerequisite(), new UnityTimeProvider());
        }

        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
    }
}