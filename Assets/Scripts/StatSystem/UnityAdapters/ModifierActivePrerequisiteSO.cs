using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    public abstract class ModifierActivePrerequisiteSO : ScriptableObject
    {
        public Modifier.ActivePrerequisite GetActivePrerequisite() => ActivePrerequisite;

        protected abstract bool ActivePrerequisite(Modifier.Contexts contexts, Modifier.IExpireTrigger trigger);
    }
}