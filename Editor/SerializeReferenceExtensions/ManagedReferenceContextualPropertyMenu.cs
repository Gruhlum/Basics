// NOTE: managedReferenceValue getter is available only in Unity 2021.3 or later.
#if UNITY_2021_3_OR_NEWER
using System;
using UnityEditor;
using UnityEngine;

namespace MackySoft.SerializeReferenceExtensions.Editor
{
    public static class ManagedReferenceContextualPropertyMenu
    {

        private const string kCopiedPropertyPathKey = "SerializeReferenceExtensions.CopiedPropertyPath";
        private const string kClipboardKey = "SerializeReferenceExtensions.CopyAndPasteProperty";

        private static readonly GUIContent kPasteContent = new GUIContent("Paste Property");
        private static readonly GUIContent kNewInstanceContent = new GUIContent("New Instance");
        private static readonly GUIContent kResetAndNewInstanceContent = new GUIContent("Reset and New Instance");

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.contextualPropertyMenu += OnContextualPropertyMenu;
        }

        private static void OnContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                // NOTE: When the callback function is called, the SerializedProperty is rewritten to the property that was being moused over at the time,
                // so a new SerializedProperty instance must be created.
                SerializedProperty clonedProperty = property.Copy();

                menu.AddItem(new GUIContent($"Copy \"{property.propertyPath}\" property"), false, Copy, clonedProperty);

                string copiedPropertyPath = SessionState.GetString(kCopiedPropertyPathKey, string.Empty);
                if (!string.IsNullOrEmpty(copiedPropertyPath))
                {
                    menu.AddItem(new GUIContent($"Paste \"{copiedPropertyPath}\" property"), false, Paste, clonedProperty);
                }
                else
                {
                    menu.AddDisabledItem(kPasteContent);
                }

                menu.AddSeparator("");

                bool hasInstance = clonedProperty.managedReferenceValue != null;
                if (hasInstance)
                {
                    menu.AddItem(kNewInstanceContent, false, NewInstance, clonedProperty);
                    menu.AddItem(kResetAndNewInstanceContent, false, ResetAndNewInstance, clonedProperty);
                }
                else
                {
                    menu.AddDisabledItem(kNewInstanceContent);
                    menu.AddDisabledItem(kResetAndNewInstanceContent);
                }
            }
        }

        private static void Copy(object customData)
        {
            SerializedProperty property = (SerializedProperty)customData;
            string json = JsonUtility.ToJson(property.managedReferenceValue);
            SessionState.SetString(kCopiedPropertyPathKey, property.propertyPath);
            SessionState.SetString(kClipboardKey, json);
        }

        private static void Paste(object customData)
        {
            SerializedProperty property = (SerializedProperty)customData;
            string json = SessionState.GetString(kClipboardKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            Undo.RecordObject(property.serializedObject.targetObject, "Paste Property");
            JsonUtility.FromJsonOverwrite(json, property.managedReferenceValue);
            property.serializedObject.ApplyModifiedProperties();
        }

        private static void NewInstance(object customData)
        {
            SerializedProperty property = (SerializedProperty)customData;
            string json = JsonUtility.ToJson(property.managedReferenceValue);

            Undo.RecordObject(property.serializedObject.targetObject, "New Instance");
            property.managedReferenceValue = JsonUtility.FromJson(json, property.managedReferenceValue.GetType());
            property.serializedObject.ApplyModifiedProperties();

            Debug.Log($"Create new instance of \"{property.propertyPath}\".");
        }

        private static void ResetAndNewInstance(object customData)
        {
            SerializedProperty property = (SerializedProperty)customData;

            Undo.RecordObject(property.serializedObject.targetObject, "Reset and New Instance");
            property.managedReferenceValue = Activator.CreateInstance(property.managedReferenceValue.GetType());
            property.serializedObject.ApplyModifiedProperties();

            Debug.Log($"Reset property and created new instance of \"{property.propertyPath}\".");
        }
    }
}
#endif