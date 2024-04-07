using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Displays
{
    public class InputDisplay : MonoBehaviour
    {
        [SerializeField] protected GameObject inputGO = default;

        [SerializeField] protected Button confirmBtn = default;
        [SerializeField] protected TMP_InputField inputField = default;
        [SerializeField] protected TMP_Text headerGUI = default;
        public bool AllowEnterKey
        {
            get
            {
                return allowEnterKey;
            }
            private set
            {
                allowEnterKey = value;
            }
        }
        [SerializeField] private bool allowEnterKey = true;


        protected int minimumLength = 2;

        public event Action<string> OnInputConfirmed;
        public UnityEvent<string> InputConfirmed;


        void Update()
        {
            if (AllowEnterKey && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
            {
                if (confirmBtn.interactable)
                {
                    Confirm_Clicked();
                }
            }
        }

        public void Show()
        {
            inputGO.SetActive(true);
            inputField.Select();
            confirmBtn.interactable = false;
        }
        public void Show(string text)
        {
            Show();
            inputField.text = text;
            UpdateConfirmButton();
        }
        public void Show(string text, string header)
        {
            Show(text);
            headerGUI.text = header;
        }
        public void InputField_TextChanged(string text)
        {
            string result = string.Concat(text.Where(char.IsLetterOrDigit));
            if (result != text)
            {
                inputField.text = result;
            }
            UpdateConfirmButton();
        }

        protected virtual bool CheckIfValid()
        {
            return inputField.text.Trim().Length >= minimumLength;
        }

        protected void UpdateConfirmButton()
        {
            if (CheckIfValid())
            {
                confirmBtn.interactable = true;
            }
            else confirmBtn.interactable = false;
        }

        public void Confirm_Clicked()
        {
            OnInputConfirmed?.Invoke(inputField.text);
            InputConfirmed?.Invoke(inputField.text);
            inputGO.SetActive(false);
            inputField.text = null;
        }
    }
}