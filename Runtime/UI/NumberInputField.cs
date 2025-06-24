using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace HexTecGames.Basics
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NumberInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField = default;
        [Space]
        public UnityEvent<int> OnValueChanged;

        public string Text
        {
            get
            {
                return inputField.text;
            }
            set
            {
                inputField.text = value;
            }
        }

        private string lastInput;

        public TMP_InputField InputField
        {
            get
            {
                return this.inputField;
            }
            private set
            {
                this.inputField = value;
            }
        }

        private void Reset()
        {
            InputField = GetComponent<TMP_InputField>();
            UnityEventTools.AddPersistentListener(InputField.onValueChanged, InputChanged);
        }

        private void Start()
        {
            lastInput = InputField.text;
        }

        public void Select()
        {
            inputField.Select();
        }

        public void SetTextWithoutNotify(int input)
        {
            SetTextWithoutNotify(input.ToString());
        }
        public void SetTextWithoutNotify(string input)
        {
            InputField.SetTextWithoutNotify(input);
        }
        private void InputChanged(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            if (input.All(char.IsNumber))
            {
                lastInput = input;
                OnValueChanged?.Invoke(Convert.ToInt32(input));
            }
            else InputField.SetTextWithoutNotify(lastInput);
        }
    }
}