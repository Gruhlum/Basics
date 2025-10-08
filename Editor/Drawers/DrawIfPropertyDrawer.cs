using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    /// <summary>
    /// Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
    /// </summary>
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfDrawer : PropertyDrawer
    {
        private DrawIfAttribute drawIf;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShouldDraw(property) && drawIf.disablingType == DrawIfAttribute.DisablingType.DontDraw)
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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldDraw(property))
            {
                if (drawIf.disablingType == DrawIfAttribute.DisablingType.ReadOnly)
                {
                    GUI.enabled = false;
                    DrawProperty(property, position, label);
                    GUI.enabled = true;
                }
                //Skip drawing entirely if set to DontDraw
                return;
            }

            DrawProperty(property, position, label);
        }

        private object GetDeclaringObject(SerializedProperty property)
        {
            var path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            var elements = path.Split('.');

            // Walk down to the declaring object (second-to-last element)
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var name = element.Substring(0, element.IndexOf("["));
                    var index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, name, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }

            return obj;
        }

        private object GetValue(object source, string name, int index = -1)
        {
            if (source == null) return null;

            var type = source.GetType();
            var field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null) return null;

            var value = field.GetValue(source);
            if (index >= 0 && value is IList list)
            {
                return list[index];
            }

            return value;
        }

        private void DrawProperty(SerializedProperty property, Rect position, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.propertyType == SerializedPropertyType.Generic && property.isArray)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else if (property.propertyType == SerializedPropertyType.Generic)
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

        private bool ShouldDraw(SerializedProperty property)
        {
            if (drawIf == null)
            {
                drawIf = attribute as DrawIfAttribute;
                if (drawIf == null)
                {
                    Debug.LogError("DrawIfDrawer: Attribute is not of type DrawIfAttribute.");
                    return true;
                }
            }

            var target = GetDeclaringObject(property);
            var targetType = target.GetType();

            FieldInfo field = null;
            PropertyInfo prop = null;

            // Walk up inheritance chain to find the field or property
            while (targetType != null)
            {
                field = targetType.GetField(drawIf.comparedPropertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field != null) break;

                prop = targetType.GetProperty(drawIf.comparedPropertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (prop != null) break;

                targetType = targetType.BaseType;
            }

            if (field == null && prop == null)
            {
                Debug.LogError($"DrawIf: Could not find field or property '{drawIf.comparedPropertyName}' on {target.GetType().Name} or its base types.");
                return true;
            }

            object value = field != null
                ? field.GetValue(target)
                : prop.GetValue(target);

            bool result = value?.Equals(drawIf.comparedValue) ?? false;
            return drawIf.reverse ? !result : result;
        }
    }
}