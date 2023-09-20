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

        public void SetText(int value)
        {
            TextGUI.text = value.ToString(formatString);
        }
        public void SetText(string text)
        {
            TextGUI.text = text.ToString();
        }

        public void SetFormatString(string format)
        {
            formatString = format;
        }
    }
}