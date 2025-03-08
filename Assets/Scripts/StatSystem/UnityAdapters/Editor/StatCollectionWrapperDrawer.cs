using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StatSystem.UnityAdapters.Editor
{
    [CustomPropertyDrawer(typeof(StatCollectionWrapper))]
    public class StatCollectionWrapperDrawer : PropertyDrawer
    {
        private readonly float _warningBoxHeight = EditorGUIUtility.singleLineHeight * 2;
        private const string StatsPropName = "stats";
        private const string StatTypePropName = "type";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
        
            SerializedProperty statsProp = property.FindPropertyRelative(StatsPropName);

            Rect rect = position;
            rect.height = _warningBoxHeight;

            // Detect duplicate StatType values
            bool hasDuplicates = HasDuplicateStatTypes(statsProp, out var index);

            if (hasDuplicates)
            {
                if (Enum.IsDefined(typeof(StatType), index))
                {
                    var type = (StatType)index;
                    EditorGUI.HelpBox(rect, $"Found multiple {type}s. Duplicates will be ignored!", MessageType.Warning);
                }
                else
                {
                    EditorGUI.HelpBox(rect, $"Found undefined duplicate! How did this happen?", MessageType.Error);
                }
                
                rect.y += _warningBoxHeight + 5; // Add space for the warning box
            }

            // Draw the array property
            EditorGUI.PropertyField(rect, statsProp, label, true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty statsProp = property.FindPropertyRelative(StatsPropName);
            bool hasDuplicates = HasDuplicateStatTypes(statsProp, out _);

            float height = EditorGUI.GetPropertyHeight(statsProp, true);

            if (hasDuplicates)
            {
                height += _warningBoxHeight + 5; // Extra height for warning box
            }

            return height;
        }

        /// <summary>
        /// Checks if there are duplicate StatType values in the array.
        /// </summary>
        private bool HasDuplicateStatTypes(SerializedProperty statsProp, out int index)
        {
            HashSet<int> uniqueTypes = new HashSet<int>(); // Using int because enums are stored as int

            for (int i = 0; i < statsProp.arraySize; i++)
            {
                SerializedProperty statElement = statsProp.GetArrayElementAtIndex(i);
                SerializedProperty typeProp = statElement.FindPropertyRelative(StatTypePropName);

                if (typeProp == null) continue;
                
                if (!uniqueTypes.Add(typeProp.enumValueIndex))
                {
                    index = typeProp.enumValueIndex;
                    return true; // Found a duplicate
                }
            }
            index = -1;
            return false;
        }
    }
}