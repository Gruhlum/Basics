using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Object = UnityEngine.Object;


namespace HexTecGames.Basics.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : UnityEditor.Editor
    {
        private Dictionary<string, List<SerializedPropertyWrapper>> groupedProperties;
        private readonly Dictionary<string, object> methodInputs = new();

        private void OnEnable()
        {
            if (target == null)
            {
                return;
            }
            groupedProperties = new Dictionary<string, List<SerializedPropertyWrapper>>();
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // Skip script reference

            var targetType = target.GetType();

            while (iterator.NextVisible(false))
            {
                var fieldInfo = targetType.GetField(iterator.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var attr = fieldInfo?.GetCustomAttribute<InspectorOrderAttribute>();
                int order = attr?.Order ?? int.MaxValue;
                string header = attr?.Header ?? "__NO_HEADER__";

                if (!groupedProperties.ContainsKey(header))
                    groupedProperties[header] = new List<SerializedPropertyWrapper>();

                groupedProperties[header].Add(new SerializedPropertyWrapper(iterator.Copy(), order, header));
            }

            // Sort each group by order
            foreach (var group in groupedProperties.Values)
                group.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        public override void OnInspectorGUI()
        {
            if (target == null)
            {
                return;
            }

            serializedObject.Update();

            DisplayHeaders();
            DisplayInspectorButtons();

            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayInspectorButtons()
        {
            var targetType = target.GetType();
            var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<InspectorButtonAttribute>();
                if (attr == null) continue;


                bool showButton = attr.Mode switch
                {
                    ButtonMode.Always => true,
                    ButtonMode.PlayModeOnly => Application.isPlaying,
                    ButtonMode.EditModeOnly => !Application.isPlaying,
                    _ => true
                };

                var parameters = method.GetParameters();

                Type paramType;
                if (parameters.Count() == 0)
                {
                    paramType = default;
                }
                else paramType = parameters[0].ParameterType;
                var methodKey = method.Name;

                if (!methodInputs.ContainsKey(methodKey))
                {
                    if (attr.HasDefaultValue && attr.DefaultValue.GetType() == paramType)
                    {
                        methodInputs[methodKey] = attr.DefaultValue;
                    }
                    else methodInputs[methodKey] = GetDefaultValue(paramType);
                }

                if (showButton)
                {
                    if (parameters.Count() > 0)
                    {
                        EditorGUILayout.BeginHorizontal();

                        if (GUILayout.Button(ObjectNames.NicifyVariableName(method.Name), GUILayout.Width(EditorGUIUtility.labelWidth)))
                        {
                            method.Invoke(target, new[] { methodInputs[methodKey] });
                        }
                        methodInputs[methodKey] = DrawInputField(methodInputs[methodKey], paramType);

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button(ObjectNames.NicifyVariableName(method.Name)))
                        {
                            method.Invoke(target, null);
                        }
                    }
                }
            }
        }

        private void DisplayHeaders()
        {
            foreach (var kvp in groupedProperties)
            {
                string header = kvp.Key;
                var properties = kvp.Value;

                if (header != "__NO_HEADER__")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
                }

                foreach (var wrapper in properties)
                {
                    try
                    {
                        EditorGUILayout.PropertyField(wrapper.Property, true);
                    }
                    catch (ExitGUIException)
                    {
                        throw; // Let Unity handle it
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error drawing property {wrapper.Property.name}: {ex}");
                    }
                }
            }
        }

        private object GetDefaultValue(Type type)
        {
            if (type == null) return null;
            if (type == typeof(int)) return 0;
            if (type == typeof(float)) return 0f;
            if (type == typeof(string)) return "";
            if (type == typeof(Vector2)) return Vector2.zero;
            if (type == typeof(Vector3)) return Vector3.zero;
            if (type == typeof(bool)) return false;
            if (type == typeof(Color)) return Color.white;
            if (typeof(Object).IsAssignableFrom(type)) return null;
            if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
            return null;
        }

        private object DrawInputField(object currentValue, Type type)
        {
            if (type == typeof(int))
                return EditorGUILayout.IntField((int)currentValue);
            if (type == typeof(float))
                return EditorGUILayout.FloatField((float)currentValue);
            if (type == typeof(string))
                return EditorGUILayout.TextField((string)currentValue);
            if (type == typeof(Vector2))
                return EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)currentValue);
            if (type == typeof(Vector3))
                return EditorGUILayout.Vector3Field(GUIContent.none, (Vector3)currentValue);
            if (type == typeof(bool))
                return EditorGUILayout.Toggle((bool)currentValue);
            if (type == typeof(Color))
                return EditorGUILayout.ColorField((Color)currentValue);
            if (typeof(Object).IsAssignableFrom(type))
                return EditorGUILayout.ObjectField((Object)currentValue, type, true);
            if (type.IsEnum)
                return EditorGUILayout.EnumPopup((Enum)currentValue);

            EditorGUILayout.LabelField($"Unsupported type: {type.Name}");
            return currentValue;
        }

        private class SerializedPropertyWrapper
        {
            public SerializedProperty Property;
            public int Order;
            public string Header;

            public SerializedPropertyWrapper(SerializedProperty property, int order, string header)
            {
                Property = property;
                Order = order;
                Header = header;
            }
        }
    }
}