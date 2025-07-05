using HexTecGames.Basics.UI;
using UnityEditor;
using UnityEditor.UI;

namespace HexTecGames
{
    [CustomEditor(typeof(TableController), true)]
    public class TableControllerEditor : HorizontalOrVerticalLayoutGroupEditor
    {
        private SerializedProperty orientation;
        private SerializedProperty fitMode;
        private SerializedProperty contentItems;
        private SerializedProperty useHeader;
        private SerializedProperty header;

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