using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames
{
    public static class SceneUtility
    {
        public static void LoadNextScene()
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index >= SceneManager.sceneCount)
            {
                index = 0;
            }
            SceneManager.LoadScene(index);
        }
        public static void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        public static void QuitGame()
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