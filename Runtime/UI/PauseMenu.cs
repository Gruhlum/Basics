using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class PauseMenu : MenuController
	{
        [SerializeField] private GameObject menuGO = default;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuGO.SetActive(!menuGO.activeInHierarchy);
            }
        }
    }
}