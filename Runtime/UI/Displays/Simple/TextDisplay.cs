using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [System.Serializable]
    public class TextDisplay : MonoBehaviour
    {
        public TMP_Text TextGUI
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
        [SerializeField] private TMP_Text textGUI;

        public event Action<string> OnTextChanged;

        private void Reset()
        {
            textGUI = GetComponentInChildren<TMP_Text>();
        }

        public void SetText(int value)
        {
            SetText(value.ToString());
        }
        public void SetText(string text)
        {
            TextGUI.text = text;
            OnTextChanged?.Invoke(text);
        }
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}