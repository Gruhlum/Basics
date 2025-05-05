using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HexTecGames.Basics.UI;
using UnityEditor.UI;
using HexTecGames.Basics;

namespace HexTecGames
{
    [CustomEditor(typeof(TableController), true)]
    public class TableControllerEditor : HorizontalOrVerticalLayoutGroupEditor
    {
        SerializedProperty orientation;
        SerializedProperty fitMode;
        SerializedProperty contentItems;
        SerializedProperty useHeader;
        SerializedProperty header;

        protected override void OnEnable()
        {
            base.OnEnable();
            orientation = serializedObject.FindProperty("orientation");
            fitMode = serializedObject.FindProperty("fitMode");
            contentItems = serializedObject.FindProperty("contentItems");
            useHeader = serializedObject.FindProperty("useHeader");
            header = serializedObject.FindProperty("header");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(orientation, true);
            EditorGUILayout.PropertyField(fitMode, true);
            EditorGUILayout.PropertyField(useHeader, true);
            if (useHeader.boolValue)
            {
                EditorGUILayout.PropertyField(header, true);
            }
            EditorGUILayout.PropertyField(contentItems, true);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}