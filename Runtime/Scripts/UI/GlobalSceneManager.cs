using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexTecGames.Basics
{
	public class GlobalSceneManager : MonoBehaviour
	{
		public static void ChangeScene(int index)
		{
			SceneManager.LoadScene(index);
		}
        public static void ChangeScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}