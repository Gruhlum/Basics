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
		

		public void Setup(IntValue intValue)
        {
            textGUI.text = intValue.Value.ToString();
            intValue.ValueChanged += IntValue_ValueChanged;
        }

        private void IntValue_ValueChanged(int value)
        {
            textGUI.text = value.ToString();
        }
    }
}