using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics
{
	public class DebugHelper : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI textGUI = default;

		private static event Action<string> OnMsgRecieved;

        private void Awake()
        {
            OnMsgRecieved += DebugHelper_OnMsgRecieved;
        }

        private void DebugHelper_OnMsgRecieved(string msg)
        {
            textGUI.text = msg;
        }

        public static void SendDebugMessage(string msg)
		{
			OnMsgRecieved?.Invoke(msg);

        }
	}
}