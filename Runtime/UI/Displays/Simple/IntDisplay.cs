using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Exile
{
	[System.Serializable]
	public class IntDisplay
	{
		[SerializeField] private TextMeshProUGUI textGUI;
		
		public void UpdateText(int intValue)
        {
            textGUI.text = intValue.ToString();
        }
    }
}