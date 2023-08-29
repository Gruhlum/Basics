using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class TextDisplay
    {
        public TextMeshProUGUI TextGUI
        {
            get
            {
                return textGUI;
            }
            private set
            {
                textGUI = value;
            }
        }
        [SerializeField] private TextMeshProUGUI textGUI;

        [SerializeField] private string formatString = default;

        public void UpdateText(int value)
        {
            TextGUI.text = value.ToString(formatString);
        }
        public void UpdateText(string text)
        {
            TextGUI.text = text.ToString();
        }

        public void SetFormatString(string format)
        {
            formatString = format;
        }
    }
}