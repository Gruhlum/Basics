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

        protected int minimumLength = 2;

        public event Action<InputData> OnInputConfirmed;

        public void Show()
        {
            inputGO.SetActive(true);
            inputField.Select();
            confirmBtn.interactable = false;
        }
        public void Show(string header)
        {
            Show();
            headerGUI.text = header;
        }
        public void Show(string header, string text)
        {
            Show(header);
            inputField.text = text;
            UpdateConfirmButton();
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

        protected virtual InputData GenerateInputData()
        {
            return new InputData(inputField.text);
        }
        public void Confirm_Clicked()
        {
            OnInputConfirmed?.Invoke(GenerateInputData());
            inputGO.SetActive(false);
            inputField.text = null;
        }
    }
}