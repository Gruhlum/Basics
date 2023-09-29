using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public class RadioButtonController : MonoBehaviour
    {
        [SerializeField] private List<RadioButton> buttons = default;

        [SerializeField] private RadioButton startButton = default;

        public event Action<RadioButton> OnRadioButtonClicked;

        private void Reset()
        {
            buttons = GetComponentsInChildren<RadioButton>().ToList();
        }

        private void Awake()
        {
            foreach (var button in buttons)
            {
                button.OnClicked += Button_OnClicked;
            }
        }
        private void Start()
        {
            if (startButton != null)
            {
                startButton.Active = true;
            }
        }

        private void Button_OnClicked(RadioButton radioBtn)
        {
            foreach (var btn in buttons)
            {
                btn.Active = btn == radioBtn;
            }
            OnRadioButtonClicked?.Invoke(radioBtn);
        }
    }
}