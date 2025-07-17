using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames.Basics.Editor
{
    [Overlay(typeof(SceneView), "Scene Selection")]
    public class SceneSelectionOverlay : ToolbarOverlay
    {

        SceneSelectionOverlay() : base(SceneDropDown.ID)
        {
        }

        [EditorToolbarElement(ID, typeof(SceneView))]
        class SceneDropDown : EditorToolbarDropdown, IAccessContainerWindow
        {
            public const string ID = "SceneSelectionOverlay/SceneDropdown";

            public EditorWindow containerWindow { get; set; }

            SceneDropDown()
            {
                text = "Scenes";
                tooltip = "Select a Scene to load";

                clicked += SceneDropDownToggle_dropdownClicked;
            }

            private void SceneDropDownToggle_dropdownClicked()
            {
                GenericMenu menu = new GenericMenu();

                Scene currentScene = SceneManager.GetActiveScene();

                var results = AssetDatabase.FindAssets("t:scene, a:assets", null);

                for (int i = 0; i < results.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(results[i]);
                    string name = Path.GetFileNameWithoutExtension(path);

                    menu.AddItem(new GUIContent(name), name == currentScene.name, () => OpenScene(currentScene, path));
                }

                menu.ShowAsContext();
            }

            private void OpenScene(Scene currentScene, string path)
            {
                if (currentScene.isDirty)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
                else EditorSceneManager.OpenScene(path);
            }
        }
    }
}