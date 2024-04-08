using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HexTecGames.Basics
{
	public class TextDisplay : MonoBehaviour
	{
		[SerializeField] private TMP_Text textGUI = default;

		public void SetText(string text)
		{
			textGUI.text = text;
			gameObject.SetActive(true);
		}
	}
}