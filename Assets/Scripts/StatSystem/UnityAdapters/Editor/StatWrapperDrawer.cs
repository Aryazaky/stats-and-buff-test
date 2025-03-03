using UnityEditor;
using UnityEngine;

namespace StatSystem.UnityAdapters.Editor
{
    [CustomPropertyDrawer(typeof(StatWrapper))]
    public class StatWrapperDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            SerializedProperty typeProp = property.FindPropertyRelative("type");
            SerializedProperty valueProp = property.FindPropertyRelative("value");

            SerializedProperty hasMinProp = property.FindPropertyRelative("hasMin");
            SerializedProperty minProp = property.FindPropertyRelative("min");

            SerializedProperty hasMaxProp = property.FindPropertyRelative("hasMax");
            SerializedProperty maxProp = property.FindPropertyRelative("max");

            SerializedProperty precisionProp = property.FindPropertyRelative("precision");
            
            bool isDefault = typeProp.enumValueIndex == 0 
                             && valueProp.floatValue == 0f 
                             && hasMinProp.boolValue == false 
                             && hasMaxProp.boolValue == false 
                             && precisionProp.intValue == 0; 

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;

            if (isDefault)
            {
                EditorGUI.HelpBox(rect, "Default (Uninitialized?)", MessageType.Warning);
    
                rect.y += EditorGUIUtility.singleLineHeight + 5;
            }

            EditorGUI.PropertyField(rect, typeProp);

            rect.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(rect, valueProp);

            rect.y += EditorGUIUtility.singleLineHeight + 2;
            hasMinProp.boolValue = EditorGUI.ToggleLeft(new Rect(rect.x, rect.y, 50, rect.height), "Min", hasMinProp.boolValue);
            if (hasMinProp.boolValue)
            {
                EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60, rect.height), minProp, GUIContent.none);
            }

            rect.y += EditorGUIUtility.singleLineHeight + 2;
            hasMaxProp.boolValue = EditorGUI.ToggleLeft(new Rect(rect.x, rect.y, 50, rect.height), "Max", hasMaxProp.boolValue);
            if (hasMaxProp.boolValue)
            {
                EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60, rect.height), maxProp, GUIContent.none);
            }

            rect.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(rect, precisionProp);
            
            // If any value changed, apply validation
            if (EditorGUI.EndChangeCheck())
            {
                float value = valueProp.floatValue;
                int precision = precisionProp.intValue;
                bool hasMin = hasMinProp.boolValue;
                bool hasMax = hasMaxProp.boolValue;
                float min = hasMin ? minProp.floatValue : float.MinValue;
                float max = hasMax ? maxProp.floatValue : float.MaxValue;

                value = Mathf.Clamp(value, min, max); // Enforce Min & Max
                precision = Mathf.Clamp(precision, 0, 15); // The max rounding digits before error
                value = (float)System.Math.Round(value, precision); // Apply Precision

                valueProp.floatValue = value; // Save it back
                precisionProp.intValue = precision;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isDefault = property.FindPropertyRelative("type").enumValueIndex == 0 &&
                             property.FindPropertyRelative("value").floatValue == 0f &&
                             property.FindPropertyRelative("hasMin").boolValue == false &&
                             property.FindPropertyRelative("hasMax").boolValue == false &&
                             property.FindPropertyRelative("precision").intValue == 0;

            // Base height for 5 fields
            float height = (EditorGUIUtility.singleLineHeight + 2) * 5;

            // Add extra height for HelpBox if needed
            if (isDefault)
            {
                height += EditorGUIUtility.singleLineHeight + 5;
            }

            return height;
        }
    }
}