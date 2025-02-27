using HexTecGames.HotkeySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public class RadioButtonController : MonoBehaviour
    {
        public RadioButton SelectedButton
        {
            get
            {
                return selectedButton;
            }
            private set
            {
                selectedButton = value;
            }
        }
        private RadioButton selectedButton;

        [SerializeField] protected List<RadioButton> buttons = default;
        [SerializeField] private RadioButton startButton = default;

        public event Action<RadioButton> OnRadioButtonClicked;

        protected virtual void Reset()
        {
            buttons = GetComponentsInChildren<RadioButton>().ToList();

            if (buttons != null && buttons.Count > 0)
            {
                startButton = buttons[0];
            }
        }

        protected virtual void Awake()
        {
            foreach (var button in buttons)
            {
                button.OnClicked += Button_OnClicked;
            }
        }
        protected virtual void Start()
        {
            if (startButton != null)
            {
                startButton.SetActive(true);
                SelectedButton = startButton;
            }
        }
        private void Button_OnClicked(RadioButton radioBtn)
        {
            SelectButton(radioBtn);
        }
        protected virtual void SelectButton(RadioButton radioBtn, bool notify = true)
        {
            foreach (var btn in buttons)
            {
                btn.SetActive(btn == radioBtn);
            }
            SelectedButton = radioBtn;

            if (notify)
            {
                OnRadioButtonClicked?.Invoke(radioBtn);
            }
        }
    }
}