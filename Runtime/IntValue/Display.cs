using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Exile
{
    [System.Serializable]
    public class TextDisplay
	{
        [SerializeField] private TextMeshProUGUI textUGUI = default;
        public void UpdateText(string text)
        {
            textUGUI.text = text;
        }
        public void UpdateText(int value)
        {
            textUGUI.text = value.ToString();
        }
	}
}