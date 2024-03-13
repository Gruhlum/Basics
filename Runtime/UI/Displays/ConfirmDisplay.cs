using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI.Displays
{
	public class ConfirmDisplay : MonoBehaviour
	{
		[SerializeField] private TMP_Text textGUI = default;

		public event Action OnConfirmClicked;
		public event Action OnCancelClicked;


		public void Setup(string text)
		{
			textGUI.text = text;
			gameObject.SetActive(true);
		}
		public void ConfirmClicked()
		{
			OnConfirmClicked?.Invoke();
		}
        public void CancelClicked()
        {
			OnCancelClicked?.Invoke();
        }
    }
}