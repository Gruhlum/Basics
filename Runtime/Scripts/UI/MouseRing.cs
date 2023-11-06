using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
	public class MouseRing : FillRing
	{
        private void Update()
        {
            SetPosition(Camera.main.GetMousePosition());
        }
    }
}