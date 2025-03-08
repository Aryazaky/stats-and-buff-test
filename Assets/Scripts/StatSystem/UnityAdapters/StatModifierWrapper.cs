using System;
using StatSystem.Modifiers;
using UnityEngine;

namespace StatSystem.UnityAdapters
{
    [Serializable]
    public struct StatModifierWrapper : IWrapper<Modifier>
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private int age;
        [SerializeField] private int priority;
        [SerializeField] private bool isExpired;
        
        public void Update(Modifier obj)
        {
            throw new NotImplementedException();
        }

        public Modifier ToOriginal()
        {
            throw new NotSupportedException();
        }
    }
}