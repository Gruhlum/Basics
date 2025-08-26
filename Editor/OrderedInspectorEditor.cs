using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace HexTecGames.Basics.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class OrderedInspectorEditor : UnityEditor.Editor
    {
        private Dictionary<string, List<SerializedPropertyWrapper>> groupedProperties;

        private void OnEnable()
        {
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
            serializedObject.Update();

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
                    EditorGUILayout.PropertyField(wrapper.Property, true);
                }
            }

            serializedObject.ApplyModifiedProperties();
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