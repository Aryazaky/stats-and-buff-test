using UnityEditor;
using UnityEngine;

namespace StatSystem.UnityAdapter.Editor
{
    [CustomPropertyDrawer(typeof(StatWrapper))]
    public class StatWrapperDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty typeProp = property.FindPropertyRelative("Type");
            SerializedProperty valueProp = property.FindPropertyRelative("Value");

            SerializedProperty hasMinProp = property.FindPropertyRelative("HasMin");
            SerializedProperty minProp = property.FindPropertyRelative("Min");

            SerializedProperty hasMaxProp = property.FindPropertyRelative("HasMax");
            SerializedProperty maxProp = property.FindPropertyRelative("Max");

            SerializedProperty precisionProp = property.FindPropertyRelative("Precision");

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;

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

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + 2) * 5;
        }
    }
}