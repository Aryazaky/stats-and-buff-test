using System;
using System.Linq;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [Serializable]
    public struct ModifierWrapper : IWriteOnlyWrapper<(Modifier modifier, IAssetMetadata metadata)>
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private int priority;
        [SerializeField] private bool isExpired;
        [SerializeField] private float age;
        [SerializeField] private bool hasTicks;
        [SerializeField] private int totalTicksElapsed;

        public ModifierWrapper((Modifier modifier, IAssetMetadata metadata) obj)
        {
            name = obj.metadata.Name;
            description = obj.metadata.Description;
            icon = obj.metadata.Icon;
            var modifier = obj.modifier;
            priority = modifier.Priority;
            isExpired = modifier.IsExpired;
            age = modifier.Age;
            if (modifier is TickableModifier statModifier)
            {
                hasTicks = true;
                totalTicksElapsed = statModifier.TotalTicksElapsed;
            }
            else
            {
                hasTicks = false;
                totalTicksElapsed = 0;
            }
        }

        public ModifierWrapper(Modifier modifier, IAssetMetadata metadata)
        {
            name = metadata.Name;
            description = metadata.Description;
            icon = metadata.Icon;
            priority = modifier.Priority;
            isExpired = modifier.IsExpired;
            age = modifier.Age;
            if (modifier is TickableModifier statModifier)
            {
                hasTicks = true;
                totalTicksElapsed = statModifier.TotalTicksElapsed;
            }
            else
            {
                hasTicks = false;
                totalTicksElapsed = 0;
            }
        }

        public void Update((Modifier modifier, IAssetMetadata metadata) obj)
        {
            name = obj.metadata.Name;
            description = obj.metadata.Description;
            icon = obj.metadata.Icon;
            var modifier = obj.modifier;
            priority = modifier.Priority;
            isExpired = modifier.IsExpired;
            age = modifier.Age;
            if (modifier is TickableModifier statModifier)
            {
                hasTicks = true;
                totalTicksElapsed = statModifier.TotalTicksElapsed;
            }
            else
            {
                hasTicks = false;
                totalTicksElapsed = 0;
            }
        }
    }
}