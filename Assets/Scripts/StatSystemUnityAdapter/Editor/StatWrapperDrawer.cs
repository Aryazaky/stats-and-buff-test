using UnityEditor;
using UnityEngine;

namespace StatSystemUnityAdapter.Editor
{
    [CustomPropertyDrawer(typeof(StatWrapper))]
    public class StatWrapperDrawer : PropertyDrawer
    {
        private const string TypePropName = "type";
        private const string ValuePropName = "value";
        private const string HasMinPropType = "hasMin";
        private const string MinPropName = "min";
        private const string HasMaxPropName = "hasMax";
        private const string MaxPropName = "max";
        private const string PrecisionPropName = "precision";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            SerializedProperty typeProp = property.FindPropertyRelative(TypePropName);
            SerializedProperty valueProp = property.FindPropertyRelative(ValuePropName);

            SerializedProperty hasMinProp = property.FindPropertyRelative(HasMinPropType);
            SerializedProperty minProp = property.FindPropertyRelative(MinPropName);

            SerializedProperty hasMaxProp = property.FindPropertyRelative(HasMaxPropName);
            SerializedProperty maxProp = property.FindPropertyRelative(MaxPropName);

            SerializedProperty precisionProp = property.FindPropertyRelative(PrecisionPropName);
            
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
            bool isDefault = property.FindPropertyRelative(TypePropName).enumValueIndex == 0 &&
                             property.FindPropertyRelative(ValuePropName).floatValue == 0f &&
                             property.FindPropertyRelative(HasMinPropType).boolValue == false &&
                             property.FindPropertyRelative(HasMaxPropName).boolValue == false &&
                             property.FindPropertyRelative(PrecisionPropName).intValue == 0;

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