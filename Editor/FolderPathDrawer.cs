using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathDrawer : PropertyDrawer
    {
        static string pendingPropertyPath;
        static SerializedProperty pendingProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var path = property.stringValue;

            var buttonRect = new Rect(position.x, position.y, position.width - 80, position.height);
            var browseRect = new Rect(position.x + position.width - 75, position.y, 75, position.height);

            EditorGUI.BeginChangeCheck();
            var newPath = EditorGUI.TextField(buttonRect, label, path);
            if (EditorGUI.EndChangeCheck())
                property.stringValue = newPath;

            if (GUI.Button(browseRect, "Browse"))
            {
                pendingPropertyPath = property.propertyPath;
                pendingProperty = property.Copy(); // Copy to avoid disposal
                EditorApplication.delayCall += OpenFolderPanelDeferred;
            }
        }

        static void OpenFolderPanelDeferred()
        {
            if (pendingProperty == null) return;

            var selectedPath = UnityEditor.EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                var so = pendingProperty.serializedObject;
                so.Update();
                var prop = so.FindProperty(pendingPropertyPath);
                if (prop != null)
                {
                    string projectPath = Application.dataPath;
                    if (selectedPath.StartsWith(projectPath))
                    {
                        // Convert to relative path starting from "Assets/"
                        selectedPath = "Assets" + selectedPath.Substring(projectPath.Length);
                    }
                    prop.stringValue = selectedPath;
                    so.ApplyModifiedProperties();
                }
            }

            pendingProperty = null;
            pendingPropertyPath = null;
        }
    }
}