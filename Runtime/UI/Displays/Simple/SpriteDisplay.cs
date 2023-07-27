using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Exile
{
	[System.Serializable]
	public class SpriteDisplay
	{
		[SerializeField] private Image Image;

		public void UpdateSprite(Sprite sprite)
		{
			Image.sprite = sprite;
		}
	}
}