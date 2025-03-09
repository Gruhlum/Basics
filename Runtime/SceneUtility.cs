using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames.Basics
{
    [CreateAssetMenu(menuName = "HexTecGames/Basics/SceneUtility")]
    public class SceneUtility : ScriptableObject
    {
        public void LoadNextScene()
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index >= SceneManager.sceneCount)
            {
                index = 0;
            }
            SceneManager.LoadScene(index);
        }
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        public void QuitGame()
        {
#if (UNITY_EDITOR)
            if (Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
                return;
            }
#endif
            Application.Quit();
        }
    }
}