using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class TextDisplay : MonoBehaviour
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

        public void SetText(int value)
        {
            SetText(value.ToString());
        }
        public void SetText(string text, bool activateGO = true)
        {
            TextGUI.text = text.ToString();
            if (activateGO)
            {
                gameObject.SetActive(true);
            }
        }
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}