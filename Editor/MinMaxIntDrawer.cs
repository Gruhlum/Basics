using HexTecGames.Basics;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxInt))]
    public class MinMaxIntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float minValue = property.FindPropertyRelative("MinValue").intValue;
            float maxValue = property.FindPropertyRelative("MaxValue").intValue;

            float minLimit = property.FindPropertyRelative("MinLimit").intValue;
            float maxLimit = property.FindPropertyRelative("MaxLimit").intValue;

            if (maxLimit < minLimit)
            {
                property.FindPropertyRelative("MaxLimit").intValue = (int)minLimit;
            }

            var labelRect = new Rect(position.x, position.y, position.width, 16);
            var sliderRect = new Rect(position.x, position.y + 18, position.width, 16);
            var minLimitRect = new Rect(position.x, position.y + 36, position.width, 16);
            var maxLimitRect = new Rect(position.x, position.y + 54, position.width, 16);
            var minValueRect = new Rect(position.x, position.y + 72, position.width, 16);
            var maxValueRect = new Rect(position.x, position.y + 90, position.width, 16);

            EditorGUI.LabelField(labelRect, new GUIContent(property.name), EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minLimit, maxLimit);

            if (maxValue < minValue)
            {
                maxValue = minValue;
            }

            property.FindPropertyRelative("MinValue").intValue = (int)minValue;
            property.FindPropertyRelative("MaxValue").intValue = (int)maxValue;

            EditorGUI.PropertyField(minLimitRect, property.FindPropertyRelative("MinLimit"));
            EditorGUI.PropertyField(maxLimitRect, property.FindPropertyRelative("MaxLimit"));
            EditorGUI.PropertyField(minValueRect, property.FindPropertyRelative("MinValue"));
            EditorGUI.PropertyField(maxValueRect, property.FindPropertyRelative("MaxValue"));

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 6;
        }
    }
}