using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI.Displays
{
	[System.Serializable]
	public class InputData
	{
		public string text;

        public InputData(string text)
        {
            this.text = text;
        }
    }
}