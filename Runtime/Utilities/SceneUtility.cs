using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames.Basics
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
        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public static void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        public static void QuitGame()
        {
#if UNITY_EDITOR
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