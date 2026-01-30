using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics
{
    [CustomPropertyDrawer(typeof(InlineSOAttribute))]
    public class InlineSODrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.objectReferenceValue == null)
                return height;

            var inlineAttr = (InlineSOAttribute)attribute;
            SerializedObject so = new SerializedObject(property.objectReferenceValue);

            var prop = so.GetIterator();
            prop.NextVisible(true);

            while (prop.NextVisible(false))
            {
                if (ShouldDraw(prop, inlineAttr))
                    height += EditorGUI.GetPropertyHeight(prop, true);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            if (property.objectReferenceValue == null)
                return;

            EditorGUI.indentLevel++;

            var inlineAttr = (InlineSOAttribute)attribute;
            SerializedObject so = new SerializedObject(property.objectReferenceValue);
            so.Update();

            var prop = so.GetIterator();
            prop.NextVisible(true);

            float y = position.y + EditorGUIUtility.singleLineHeight;

            while (prop.NextVisible(false))
            {
                if (!ShouldDraw(prop, inlineAttr))
                    continue;

                float h = EditorGUI.GetPropertyHeight(prop, true);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), prop, true);
                y += h;
            }

            so.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

        private bool ShouldDraw(SerializedProperty prop, InlineSOAttribute inlineAttr)
        {
            if (!inlineAttr.ShowOnlyAttributedFields)
                return true;

            // Only show fields with [InlineSOField]
            var field = prop.serializedObject.targetObject
                .GetType()
                .GetField(prop.name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            return field != null && field.GetCustomAttributes(typeof(InlineSOFieldAttribute), true).Length > 0;
        }
    }
}
