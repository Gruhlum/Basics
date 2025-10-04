using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    [CustomPropertyDrawer(typeof(DrawIfSceneAttribute), true)]
    public class DrawIfSceneDrawer : PropertyDrawer
    {


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var drawIfSceneAttribute = attribute as DrawIfSceneAttribute;

            if (drawIfSceneAttribute.Show(property.serializedObject.targetObject))
            {
                EditorGUI.BeginProperty(position, label, property);

                if (property.propertyType == SerializedPropertyType.Generic)
                {
                    property.isExpanded = EditorGUI.Foldout(
                        new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                        property.isExpanded, label, true);

                    if (property.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        var child = property.Copy();
                        var end = property.GetEndProperty();

                        child.NextVisible(true);
                        float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                        while (child.propertyPath != end.propertyPath)
                        {
                            float height = EditorGUI.GetPropertyHeight(child, true);
                            EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), child, true);
                            y += height + EditorGUIUtility.standardVerticalSpacing;

                            if (!child.NextVisible(false)) break;
                        }

                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
                EditorGUI.EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var drawIfSceneAttribute = attribute as DrawIfSceneAttribute;

            if (!drawIfSceneAttribute.Show(property.serializedObject.targetObject))
                return 0f;

            if (property.propertyType != SerializedPropertyType.Generic)
                return EditorGUI.GetPropertyHeight(property, label, true);

            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                var child = property.Copy();
                var end = property.GetEndProperty();

                child.NextVisible(true);
                while (child.propertyPath != end.propertyPath)
                {
                    height += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
                    if (!child.NextVisible(false)) break;
                }
            }

            return height;
        }
    }
}