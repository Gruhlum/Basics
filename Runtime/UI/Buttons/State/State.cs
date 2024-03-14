using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
	[System.Serializable]
	public class State
	{
        public string text = default;
        public Sprite sprite = default;
        public UnityEvent clickEvent = default;
	}
}