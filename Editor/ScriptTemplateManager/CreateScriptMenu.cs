using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    public static class CreateScriptMenu
    {
        static string templateFolder = "Plugins/ScriptTemplates";

        [MenuItem("Assets/Create/MonoBehaviour", priority = 1, validate = false)]
        static void CreateMonoBehaviourMenuItem()
        {
            string pathToTemplate = Path.Combine(Application.dataPath, templateFolder, "MonoBehaviourTemplate.txt");
            CreateTemplate(pathToTemplate, "MonoBehaviour");
        }
        [MenuItem("Assets/Create/ScriptableObject", priority = 2)]
        static void CreateScriptableObjectMenuItem()
        {
            string pathToTemplate = Path.Combine(Application.dataPath, templateFolder, "ScriptableObjectTemplate.txt");
            CreateTemplate(pathToTemplate, "ScriptableObject");
        }
        [MenuItem("Assets/Create/SerializedClass", priority = 3)]
        static void CreateSerializedClassMenuItem()
        {
            string pathToTemplate = Path.Combine(Application.dataPath, templateFolder, "SerializedClassTemplate.txt");
            CreateTemplate(pathToTemplate, "SerializedClass");
        }
        [MenuItem("Assets/Create/Interface", priority = 4)]
        static void CreateInterfaceMenuItem()
        {
            string pathToTemplate = Path.Combine(Application.dataPath, templateFolder, "InterfaceTemplate.txt");

            CreateTemplate(pathToTemplate, "IInterface");
        }
        [MenuItem("Assets/Create/Editor", priority = 5)]
        static void CreateEditorMenuItem()
        {
            string pathToTemplate = Path.Combine(Application.dataPath, templateFolder, "EditorTemplate.txt");

            CreateTemplate(pathToTemplate, "SomeEditor");
        }

        public static string GetFolder()
        {
            Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);

            if ((selectedObjects?.Length ?? 0) > 0)
            {
                string folderPath = AssetDatabase.GetAssetPath(selectedObjects[0]);
                if (AssetDatabase.IsValidFolder(folderPath))
                {
                    return folderPath;
                }
                else if (File.Exists(folderPath))
                {
                    return Path.GetDirectoryName(folderPath);
                }
            }
            return "Assets";
        }

        static void CreateTemplate(string templatePath, string defaultName)
        {
            CreateScriptEndNameEditAction create = ScriptableObject.CreateInstance<CreateScriptEndNameEditAction>();
            create.templatePath = templatePath;
            string newPath = Path.Combine(GetFolder(), defaultName + ".cs");
            Texture2D icon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, create, newPath, icon, null);
        }
    }

    internal class CreateScriptEndNameEditAction : EndNameEditAction
    {
        public string templatePath;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            ReplacePlaceholders(pathName);
            AssetDatabase.Refresh();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(pathName);
            Selection.SetActiveObjectWithContext(obj, obj);
        }
        private void ReplacePlaceholders(string pathName)
        {
            FileInfo fileInfo = new FileInfo(pathName);
            string nameOfScript = Path.GetFileNameWithoutExtension(fileInfo.Name);

            string text = File.ReadAllText(templatePath);

            text = text.Replace("#SCRIPTNAME#", nameOfScript);
            text = text.Replace("#SCRIPTNAMEWITHOUTEDITOR#", nameOfScript.Replace("Editor", string.Empty));

            File.WriteAllText(pathName, text);
        }
    }
}